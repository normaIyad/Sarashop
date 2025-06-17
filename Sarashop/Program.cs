using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Sarashop.DataBase;
using Sarashop.Models;
using Sarashop.service;
using Sarashop.Service;
using Sarashop.Utility;
using Sarashop.Utility.DataBaseInitulizer;
using Scalar.AspNetCore;
using Stripe;
using System.Text;

namespace Sarashop
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // CORS Policy
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                    policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            // Controllers & DB
            builder.Services.AddControllers();
            builder.Services.AddDbContext<DatabaseConfigration>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            );

            // OpenAPI & Swagger
            builder.Services.AddOpenApi(); // Scalar open API
            builder.Services.AddEndpointsApiExplorer();

            // Custom Services
            builder.Services.AddScoped<IDBInitalizer, DatabaseIntalizer>();
            builder.Services.AddTransient<IEmailSender, EmailSender>();
            builder.Services.AddScoped<IBrand, BrandService>();
            builder.Services.AddScoped<IProdact, ProdactService>();
            builder.Services.AddScoped<ICatigoryService, CategoryService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<ICart, CartServices>();
            builder.Services.AddScoped<IOrderItem, OrderItemService>();
            builder.Services.AddScoped<IPasswordResetCodeService, PasswordResetCodeService>();
            builder.Services.AddScoped<IReview, service.ReviewService>();
            builder.Services.AddScoped<CartServices>();
            //Stripe
            builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
            StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

            // JWT Configuration
            var jwtSettings = builder.Configuration.GetSection("Jwt");
            var key = jwtSettings["Key"];

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine("Authentication failed: " + context.Exception.Message);
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        Console.WriteLine("Token validated for: " + context.Principal.Identity?.Name);
                        return Task.CompletedTask;
                    }
                };
            });

            // Identity (must come BEFORE setting default policy)
            builder.Services.AddIdentity<ApplecationUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = false;
            })
            .AddEntityFrameworkStores<DatabaseConfigration>()
            .AddDefaultTokenProviders();

            // Authorization - default policy to use JWT scheme
            builder.Services.AddAuthorizationBuilder()
                .SetDefaultPolicy(new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build());

            var app = builder.Build();

            // Swagger UI
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();

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

            // Debug Logging Middleware
            app.Use(async (context, next) =>
            {
                Console.WriteLine($"Request: {context.Request.Path}");
                await next();
            });

            // Init DB
            using (var scope = app.Services.CreateScope())
            {
                var service = scope.ServiceProvider.GetService<IDBInitalizer>();
                await service.IntalizeAsync();
            }
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "imgs")),
                RequestPath = "/imgs"
            });
            // Middleware pipeline
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors(MyAllowSpecificOrigins);
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
