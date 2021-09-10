using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.Identity.MongoDbCore.Models;
using HotelReservationAPI.Models.Models;
using Microsoft.AspNetCore.Identity;

namespace HotelReservationAPI.Data
{
    public static class PreSeeder
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="roleManager"></param>
        /// <param name="userManager"></param>
        /// <returns></returns>
        public static async Task Seed(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            //await context.Database.EnsureCreatedAsync();

            if (!roleManager.Roles.Any())
            {
                var listOfRoles = new List<ApplicationRole>
                {
                    new ApplicationRole(){Name = "Admin"},
                    new ApplicationRole(){Name = "Guest"},
                    new ApplicationRole(){Name = "Staff"}
                };
                foreach (var role in listOfRoles)
                {
                    await roleManager.CreateAsync(role);
                }
            }

         
            // pre-load Admin user
            if (!userManager.Users.Any())
            {
                ApplicationUser user = new ApplicationUser()
                {
                    FirstName = "Peter",
                    LastName = "Tosingh",
                    UserName = "tosin@gmail.com",
                    Email = "tosin@gmail.com",
                    PhoneNumber = "09032290095",
                    Address = "Sangotedo",
                    Gender = "Male"
                };

                var result = await userManager.CreateAsync(user, "12345Admin");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }
    }
}