using EShop.Application;
using EShop.Domain.Repositories;
using EShop.Domain.Seeders;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using EShop.Application.Services;
using EShop.Domain;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Middleware;
using Microsoft.Extensions.Options;
using EShop.Application.AutoMappers;

namespace EShopService
{
    
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            bool underMaintenance = builder.Configuration.GetValue<bool>("UnderMaintenance");


            // Add services to the container.

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => 
            {
                
                var rsa = RSA.Create();
                rsa.ImportFromPem(File.ReadAllText("../data/publickey.pem"));
                var publicKey = new RsaSecurityKey(rsa);

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "UserService",
                    ValidAudience = "Eshop",
                    IssuerSigningKey = publicKey
                };
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Administrator", policy => policy.RequireRole("Administrator"));
                options.AddPolicy("Employee", policy => policy.RequireRole("Employee"));
                options.AddPolicy("Client", policy => policy.RequireRole("Client"));
            });

            builder.Services.AddDbContext<DataContext>(options => options.UseInMemoryDatabase("TestDataBase"));

            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IProductService, ProductService>();

            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();

            builder.Services.AddScoped<ICartRepository, CartRepository>();
            builder.Services.AddScoped<ICartService, CartService>();

            builder.Services.AddScoped<ICreditCardService, CreditCardService>();
            builder.Services.AddScoped<IEShopSeeder, EShopSeeder>();

            builder.Services.AddAutoMapper(
                typeof(ProductMappingProfile),
                typeof(ProductUpdateMappingProfile),
                typeof(CategoryMappingProfile),
                typeof(CategoryUpdateMappingProfile)
                );

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            
            //builder.Services.AddCors(options => options.AddPolicy("allowAnyOriginAnyHeaderAnyMethod", policy =>
            //{
            //    policy.AllowAnyOrigin()
            //          .AllowAnyHeader()
            //          .AllowAnyMethod();
            //})
            //);

            var app = builder.Build();

            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseMiddleware<ServiceUnderMaintenanceMiddleware>(underMaintenance);
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseStaticFiles();
                app.UseSwagger();
                app.UseSwaggerUI();

                //app.UseCors("allowAnyOriginAnyHeaderAnyMethod");
                using (var scope = app.Services.CreateScope())
                {
                    var seeder = scope.ServiceProvider.GetRequiredService<IEShopSeeder>();
                    await seeder.Seed();
                    //await seeder.CleanProductsTable();
                }
            }
            
            app.UseRouting();
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<PageNotFoundMiddleware>();
            app.MapControllers();

            
            app.Run();
        } 
    }
}
