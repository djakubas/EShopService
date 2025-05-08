
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using User.Domain.Models;
using User.Application;
using User.Domain.Seeders;
using User.Domain.Repositories;
using Microsoft.EntityFrameworkCore;


namespace UserService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var jwtSettings = builder.Configuration.GetSection("Jwt");
            builder.Services.Configure<JwtSettings>(jwtSettings);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var jwtConfig = jwtSettings.Get<JwtSettings>();
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtConfig.Issuer,
                    ValidAudience = jwtConfig.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Key))
                };
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Administrator", policy => policy.RequireRole("Administrator"));
                options.AddPolicy("Employee", policy => policy.RequireRole("Employee"));
                options.AddPolicy("Client", policy => policy.RequireRole("Client"));
            });
            //
            //for testing purposes
            builder.Services.AddCors(options => options.AddPolicy("allowAnyOriginAnyHeaderAnyMethod", policy => 
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                          
                          
                })
            );
            builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
            builder.Services.AddScoped<ILoginService, LoginService>();
            builder.Services.AddScoped<IUsersSeeder, UsersSeeder>();
            builder.Services.AddDbContext<UsersDataContext>(options => options.UseInMemoryDatabase("TestDB"));


            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseCors("allowAnyOriginAnyHeaderAnyMethod");
            }

            using (var scope = app.Services.CreateScope())
                {
                    var seeder = scope.ServiceProvider.GetRequiredService<IUsersSeeder>();
                    await seeder.Seed();
                }

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
