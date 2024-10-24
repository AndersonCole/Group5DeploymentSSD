using Lab1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Lab1.Data
{
    public class DbInitializer
    {
        public static AppSecrets appSecrets { get; set; }
        /// <summary>
        /// Code mostly from github examples, values changed for this Lab and logging added
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public static async Task<int> SeedUsersAndRoles(IServiceProvider serviceProvider)
        {
            // create the database if it doesn't exist
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            Console.WriteLine("Checking roles!");
            // Check if roles already exist and exit if there are
            if (roleManager.Roles.Count() > 0)
            {
                Console.WriteLine("Roles already exist in the DB!");
                return 1;
            }

            Console.WriteLine("Seeding roles!");
            //Seeding roles
            int result = await SeedRoles(roleManager);
            if (result != 0)
            {
                Console.WriteLine("Error with seeding roles!");
                return 2;
            }

            Console.WriteLine("Checking users!");
            // Check if users already exist and exit if there are
            if (userManager.Users.Count() > 0)
            {
                Console.WriteLine("Users already exist in the DB!");
                return 3;
            }

            Console.WriteLine("Seeding users!");
            // Seed users
            result = await SeedUsers(userManager);
            if (result != 0)
            {
                Console.WriteLine("Error with seeding users!");
                return 4;
            }

            return 0;
        }

        /// <summary>
        /// Code mostly from github examples, adapted to this project, and logging added
        /// </summary>
        /// <param name="roleManager"></param>
        /// <returns></returns>
        private static async Task<int> SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            // Create Manager Role
            var result = await roleManager.CreateAsync(new IdentityRole("Manager"));
            if (!result.Succeeded)
            {
                Console.WriteLine("Error with creating manager role!");
                return 1;
            }

            // Create Employee Role
            result = await roleManager.CreateAsync(new IdentityRole("Employee"));
            if (!result.Succeeded)
            {
                Console.WriteLine("Error with creating employee role!");
                return 1;
            }

            return 0;
        }

        /// <summary>
        /// Code mostly from github example, data changed and logging added
        /// </summary>
        /// <param name="userManager"></param>
        /// <returns></returns>
        private static async Task<int> SeedUsers(UserManager<ApplicationUser> userManager)
        {
            // Create Manager User
            var managerUser = new ApplicationUser
            {
                UserName = "manager@mohawkcollege.ca",
                Email = "manager@mohawkcollege.ca",
                FirstName = "The",
                LastName = "Manager",
                PhoneNumber = "123-456-7890",
                BirthDate = DateTime.Parse("2000-01-01"),
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(managerUser, appSecrets.ManagerPassword);
            if (!result.Succeeded)
            {
                Console.WriteLine("Error with creating a manager user!");
                return 1;
            }


            result = await userManager.AddToRoleAsync(managerUser, "Manager");
            if (!result.Succeeded)
            {
                Console.WriteLine("Error with adding manager user to role!");
                return 2;
            }

            // Create Employee User
            var employeeUser = new ApplicationUser
            {
                UserName = "employee@mohawkcollege.ca",
                Email = "employee@mohawkcollege.ca",
                FirstName = "The",
                LastName = "Employee",
                PhoneNumber = "098-765-4321",
                BirthDate = DateTime.Parse("2000-02-02"),
                EmailConfirmed = true
            };
            result = await userManager.CreateAsync(employeeUser, appSecrets.EmployeePassword);
            if (!result.Succeeded)
            {
                Console.WriteLine("Error with creating Employee user!");
                return 3;
            }

            // Assign user to Employee role
            result = await userManager.AddToRoleAsync(employeeUser, "Employee");
            if (!result.Succeeded)
            {
                Console.WriteLine("Error with adding employee user to role!");
                return 4;
            }

            return 0;
        }
    }
}
