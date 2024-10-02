// Data/SeedData.cs
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Assignment1.Models;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace Assignment1.Data
{
    public static class SeedData
    {
        // AppSecrets property to store secrets injected from the configuration
        public static AppSecrets appSecrets { get; set; }

        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using (var context = serviceProvider.GetRequiredService<ApplicationDbContext>())
            {
                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger("SeedData");
                logger.LogInformation("Starting database seeding...");

                // Apply any pending migrations
                await context.Database.MigrateAsync();

                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                // Define roles
                string[] roles = new[] { "Manager", "Employee" };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        var result = await roleManager.CreateAsync(new IdentityRole(role));
                        if (result.Succeeded)
                        {
                            logger.LogInformation($"Role '{role}' created successfully.");
                        }
                        else
                        {
                            logger.LogError($"Error creating role '{role}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
                        }
                    }
                    else
                    {
                        logger.LogInformation($"Role '{role}' already exists.");
                    }
                }

                // Create a Manager user using secrets
                var managerEmail = appSecrets.Manager.Email;
                var managerPassword = appSecrets.Manager.Password;

                var managerUser = await userManager.FindByEmailAsync(managerEmail);
                if (managerUser == null)
                {
                    managerUser = new ApplicationUser
                    {
                        UserName = managerEmail, // Set UserName to Email
                        Email = managerEmail,
                        PhoneNumber = "123-456-7890", // Ensure non-null
                        FirstName = "ManagerFirstName",
                        LastName = "ManagerLastName",
                        BirthDate = new DateTime(1980, 1, 1),
                        EmailConfirmed = true // Ensure email is confirmed
                    };
                    var result = await userManager.CreateAsync(managerUser, managerPassword);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(managerUser, "Manager");
                        logger.LogInformation($"Manager user '{managerEmail}' created and assigned to 'Manager' role.");
                    }
                    else
                    {
                        logger.LogError($"Error creating Manager user '{managerEmail}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }
                }
                else
                {
                    logger.LogInformation($"Manager user '{managerEmail}' already exists.");
                }

                // Create an Employee user using secrets
                var employeeEmail = appSecrets.Employee.Email;
                var employeePassword = appSecrets.Employee.Password;

                var employeeUser = await userManager.FindByEmailAsync(employeeEmail);
                if (employeeUser == null)
                {
                    employeeUser = new ApplicationUser
                    {
                        UserName = employeeEmail, // Set UserName to Email
                        Email = employeeEmail,
                        PhoneNumber = "987-654-3210", // Ensure non-null
                        FirstName = "EmployeeFirstName",
                        LastName = "EmployeeLastName",
                        BirthDate = new DateTime(1990, 1, 1),
                        EmailConfirmed = true // Ensure email is confirmed
                    };
                    var result = await userManager.CreateAsync(employeeUser, employeePassword);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(employeeUser, "Employee");
                        logger.LogInformation($"Employee user '{employeeEmail}' created and assigned to 'Employee' role.");
                    }
                    else
                    {
                        logger.LogError($"Error creating Employee user '{employeeEmail}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }
                }
                else
                {
                    logger.LogInformation($"Employee user '{employeeEmail}' already exists.");
                }

                // Add Employers if necessary (optional)
                if (!context.Employers.Any())
                {
                    context.Employers.AddRange(
                        new Employer
                        {
                            Name = "TechCorp",
                            PhoneNumber = "123-456-7890",
                            Website = "https://www.techcorp.com",
                            IncorporatedDate = new DateTime(2000, 5, 15)
                        },
                        new Employer
                        {
                            Name = "HealthPlus",
                            PhoneNumber = "987-654-3210",
                            Website = "https://www.healthplus.com"
                        }
                    );
                    await context.SaveChangesAsync();
                    logger.LogInformation("Sample Employers added to the database.");
                }
                else
                {
                    logger.LogInformation("Employers already exist in the database.");
                }

                logger.LogInformation("Database seeding completed.");
            }
        }
    }
}
