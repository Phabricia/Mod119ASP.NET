using CursoMOD119.lib;
using Microsoft.AspNetCore.Identity;
using System.Drawing.Text;
using System.Linq.Expressions;

namespace CursoMOD119.Data.Seed
{
    public class SeedDatabase
    {
        public static void Seed(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);

        }
        private static async void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            try
            {
                var roleCheck = await roleManager.RoleExistsAsync(AppConstants.ADMIN_ROLE); //operative

                if (!roleCheck)
                {
                    var adminRole = new IdentityRole
                    {
                        Name = AppConstants.ADMIN_ROLE
                    };
                    var roleResult = await roleManager.CreateAsync(adminRole);
                }

                roleCheck = await roleManager.RoleExistsAsync(AppConstants.OPERATIVE_ROLE);

                if (!roleCheck)
                {
                    var operativeRole = new IdentityRole
                    {
                        Name = AppConstants.OPERATIVE_ROLE
                    };

                    var roleResult = await roleManager.CreateAsync(operativeRole);
                }

            }
            catch (Exception ex)
            {

            }


        }

        private async static void SeedUsers(UserManager<IdentityUser> userManager)
        {
            try
            {
                IdentityUser adminUser = new IdentityUser
                {
                    UserName = AppConstants.ADMIN_USER,
                    Email = AppConstants.ADMIN_EMAIL
                };

                var dbAdmin = await userManager.FindByNameAsync(adminUser.UserName);

                if (dbAdmin == null)
                {
                    var result = await userManager.CreateAsync(adminUser, AppConstants.ADMIN_PWD);

                    if (result.Succeeded)
                    {

                        await userManager.AddToRoleAsync(adminUser, AppConstants.ADMIN_ROLE);
                        dbAdmin = await userManager.FindByNameAsync(adminUser.UserName);
                        dbAdmin!.EmailConfirmed = true;
                        await userManager.UpdateAsync(dbAdmin);
                    }
                }

                IdentityUser operativeUser = new IdentityUser
                {
                    UserName = AppConstants.OPERATIVE_USER,
                    Email = AppConstants.OPERATIVE_EMAIL
                };

                var dbOperative = await userManager.FindByNameAsync(operativeUser.UserName);

                if (dbOperative == null)
                {
                    var result = await userManager.CreateAsync(operativeUser, AppConstants.OPERATIVE_PWD);

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(operativeUser, AppConstants.OPERATIVE_ROLE);
                    }
                }

            }
            
             catch (Exception e)
            {

            }


        }
        
    }
}
