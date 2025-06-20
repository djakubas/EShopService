
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using User.Domain.Models;
using User.Application;
using User.Domain.Seeders;
using User.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using User.Application.AutoMappers;
using AutoMapper;
using Infrastructure.Middleware;

namespace UserService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //maintenance status
            bool maintenance = builder.Configuration.GetValue<bool>("UnderMaintenance");
            
            //JWT Token Auth config
            var jwtSettings = builder.Configuration.GetSection("Jwt");
            builder.Services.Configure<JwtSettings>(jwtSettings);

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
            builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

            //Authorization Roles

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Administrator", policy => policy.RequireRole("Administrator"));
                options.AddPolicy("Employee", policy => policy.RequireRole("Employee"));
                options.AddPolicy("Client", policy => policy.RequireRole("Client"));
            });
            
            //for testing purposes
            ////builder.Services.AddCors(options => options.AddPolicy("allowAnyOriginAnyHeaderAnyMethod", policy =>
            ////    {
            ////        policy.AllowAnyOrigin()
            ////              .AllowAnyHeader()
            ////              .AllowAnyMethod();
            ////    })
            ////);

            //Identity Options
            var identityOptionsSection = builder.Configuration.GetSection("IdentityOptions");
            var identityOptions = identityOptionsSection.Get<IdentityOptions>();
            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = identityOptions!.Password.RequiredLength;
                options.Password.RequireNonAlphanumeric = identityOptions.Password.RequireNonAlphanumeric;
                options.Password.RequireDigit = identityOptions.Password.RequireDigit;
                options.Password.RequireUppercase = identityOptions.Password.RequireUppercase;
                options.Password.RequireLowercase = identityOptions.Password.RequireLowercase;
            });



            //Register and login services
            builder.Services.AddScoped<IRegisterService, RegisterService>();
            builder.Services.AddScoped<ILoginService, LoginService>();
            builder.Services.AddScoped<IPasswordHasher<UserModel>, PasswordHasher<UserModel>>();
            builder.Services.AddScoped<IPasswordValidateService, PasswordValidateService>();
            builder.Services.AddScoped<IUniqueUserValidateService, UniqueUserValidateService>();
            builder.Services.AddAutoMapper(typeof(UserMappingProfile), typeof(UserUpdateMappingProfile));

            //Seeder
            builder.Services.AddScoped<IUsersSeeder, UsersSeeder>();
            
            //DBcontext and UsersRepository
            builder.Services.AddDbContext<UsersDataContext>(options => options.UseInMemoryDatabase("TestDB"));
            builder.Services.AddScoped<IUsersRepository, UsersRepository>();
            

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseMiddleware<ServiceUnderMaintenanceMiddleware>(maintenance);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseStaticFiles();
                app.UseSwagger();
                app.UseSwaggerUI();
                //app.UseCors("allowAnyOriginAnyHeaderAnyMethod");
                using (var scope = app.Services.CreateScope())
                {
                    var seeder = scope.ServiceProvider.GetRequiredService<IUsersSeeder>();
                    await seeder.Seed();
                }
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHttpsRedirection();

            app.UseMiddleware<PageNotFoundMiddleware>();
            app.MapControllers();

            app.Run();
        }
    }
}
