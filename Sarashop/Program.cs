using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Sarashop.DataBase;
using Sarashop.Models;
using Sarashop.service;
using Sarashop.Service;
using Sarashop.Utility;
using Sarashop.Utility.DataBaseInitulizer;
using Scalar.AspNetCore;
using System.Text;

namespace Sarashop
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure CORS
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                    policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            // Add services to the container
            builder.Services.AddControllers();
            builder.Services.AddDbContext<DatabaseConfigration>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            builder.Services.AddOpenApi(); // Scalar open API
            builder.Services.AddEndpointsApiExplorer();

            // Register custom services
            // DatabaseIntalizer : IDBInitalizer
            builder.Services.AddScoped<IDBInitalizer, DatabaseIntalizer>();
            builder.Services.AddTransient<IEmailSender, EmailSender>();
            builder.Services.AddScoped<IBrand, BrandService>();
            builder.Services.AddScoped<IProdact, ProdactService>();
            builder.Services.AddScoped<ICatigoryService, CategoryService>();
            //builder.Services.AddScoped<ICart, CartServices>();
            var jwtSettings = builder.Configuration.GetSection("Jwt");

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
           .AddJwtBearer(options =>
           {
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = false,
                   ValidateAudience = false,
                   ValidateLifetime = true,
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("pX0BboXxx7FAAhJS8kfdiJJVJp7xkSAO"))
               };
           });


            builder.Services.AddAuthorization();
            // Configure Identity
            builder.Services.AddIdentity<ApplecationUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = false;
            })
            .AddEntityFrameworkStores<DatabaseConfigration>()
            .AddDefaultTokenProviders();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi(); // Swagger UI

                app.MapScalarApiReference(options =>
                {
                    options.Title = "My custom API";
                    options.Theme = ScalarTheme.Mars;
                    options.ShowSidebar = false;
                    options.DefaultHttpClient = new(ScalarTarget.CSharp, ScalarClient.HttpClient);
                    options.Authentication = new ScalarAuthenticationOptions
                    {
                        PreferredSecurityScheme = "ApiKey",
                        ApiKey = new ApiKeyOptions
                        {
                            Token = "my-api-key"
                        }
                    };
                });
            }

            // Middleware pipeline
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors(MyAllowSpecificOrigins);
            app.UseAuthentication();
            app.UseAuthorization();
            var scope = app.Services.CreateScope();
            var service = scope.ServiceProvider.GetService<IDBInitalizer>();
            await service.IntalizeAsync();
            app.MapControllers();
            app.Run();
        }
    }
}
