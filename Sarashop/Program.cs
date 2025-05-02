using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Sarashop.DataBase;
using Sarashop.Models;
using Sarashop.service;
using Sarashop.Service;
using Sarashop.Utility;
using Sarashop.Utility.DataBaseInitulizer;
using Scalar.AspNetCore;

namespace Sarashop
{
    public class Program
    {
        public static void Main(string[] args)
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
            builder.Services.AddScoped<CartServices>();
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
            app.UseAuthorization();
            app.MapControllers();
            var scope = app.Services.CreateScope();
            var service = scope.ServiceProvider.GetService<IDBInitalizer>();
            service.IntalizeAsync();

            app.Run();
        }
    }
}
