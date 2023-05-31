using Microsoft.AspNetCore.Identity;
using WA_TravelAgency_v1.Models.Enums;
using WA_TravelAgency_v1.Models.Identity;

namespace WA_TravelAgency_v1.Data
{
    public static class DbSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider service)
        {
            //Seed Roles
            var userManager = service.GetService<UserManager<ApplicationUser>>();
            var roleManager = service.GetService<RoleManager<IdentityRole>>();
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Employee.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.User.ToString()));

            // creating admin

            //var user = new ApplicationUser
            //{
            //    UserName = "admin@gmail.com",
            //    Email = "admin@gmail.com",
            //    Name = "Magdalena",
            //    EmailConfirmed = true,
            //    PhoneNumberConfirmed = true
            //};
            //var userInDb = await userManager.FindByEmailAsync(user.Email);
            //if (userInDb == null)
            //{
            //    await userManager.CreateAsync(user, "Admin@123");
            //    await userManager.AddToRoleAsync(user, Roles.Admin.ToString());
            //}
        }
    }
}
