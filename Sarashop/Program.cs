
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Sarashop.DataBase;

namespace Sarashop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddDbContext<DatabaseConfigration>(opration =>
            {
                opration.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            }
            );

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Sarashop API",
                    Version = "v1"
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseStaticFiles();
                app.UseSwagger();

                app.UseReDoc(options =>
                {
                    options.RoutePrefix = "docs";
                    options.SpecUrl = "/swagger/v1/swagger.json";
                    options.DocumentTitle = "Sarashop API Docs";
                });
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sarashop API v1");
                    c.RoutePrefix = string.Empty;
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
