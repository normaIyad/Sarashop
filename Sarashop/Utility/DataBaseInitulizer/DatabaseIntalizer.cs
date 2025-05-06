using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sarashop.DataBase;
using Sarashop.Models;

namespace Sarashop.Utility.DataBaseInitulizer
{
    public class DatabaseIntalizer : IDBInitalizer
    {
        private readonly DatabaseConfigration daatabase;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplecationUser> userManager;
        public DatabaseIntalizer(DatabaseConfigration daatabase,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplecationUser> userManager
            )
        {
            this.daatabase = daatabase;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public async Task IntalizeAsync()
        {
            try
            {
                if ((await daatabase.Database.GetPendingMigrationsAsync()).Any())
                {
                    Console.WriteLine("Applying pending migrations...");
                    await daatabase.Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Migration error: {ex.Message}");
            }

            try
            {
                if (!roleManager.Roles.Any())
                {
                    Console.WriteLine("Creating roles...");

                    await roleManager.CreateAsync(new IdentityRole(StaticData.SuperAdmin));
                    await roleManager.CreateAsync(new IdentityRole(StaticData.Admin));
                    await roleManager.CreateAsync(new IdentityRole(StaticData.Customer));
                    await roleManager.CreateAsync(new IdentityRole(StaticData.Company));

                    Console.WriteLine("Roles created successfully.");
                }

                var user = await userManager.FindByEmailAsync("admin@gmail.com");
                if (user == null)
                {
                    Console.WriteLine("Creating SuperAdmin...");
                    var newUser = new ApplecationUser
                    {
                        FirstName = "superadmin",
                        LastName = "superadd",
                        UserName = "superaddmin101",
                        Gender = Gender.Male,
                        BirthDate = new DateTime(1999, 8, 7),
                        Email = "admin@gmail.com",
                        EmailConfirmed = true
                    };
                    var result = await userManager.CreateAsync(newUser, "Admin@123");
                    //var result = await userManager.CreateAsync(newUser, "admin@1");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(newUser, "SuperAdmin");
                        Console.WriteLine("✅ SuperAdmin created and assigned.");
                    }
                    else
                    {
                        Console.WriteLine("❌ Failed to create SuperAdmin:");
                        foreach (var error in result.Errors)
                        {
                            Console.WriteLine($" - {error.Description}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("ℹ️ SuperAdmin already exists.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Initialization error: {ex.Message}");
            }
        }


    }


}

