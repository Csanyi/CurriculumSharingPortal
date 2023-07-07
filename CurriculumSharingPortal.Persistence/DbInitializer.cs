using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CurriculumSharingPortal.Persistence
{
	public static class DbInitializer
    {
        public static async Task Initialize(CurriculumSharingPortalDbContext context, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            context.Database.Migrate();

            if (context.Subjects.Any())
            {
                return;
            }

            var roles = new[] { "User", "Admin" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            string name = "admin";
            string password = "123";

            if (await userManager.FindByNameAsync(name) == null)
            {
                var admin = new Admin
                {
                    UserName = name,
                };

                await userManager.CreateAsync(admin, password);
                await userManager.AddToRoleAsync(admin, "Admin");
            }

            IList<Subject> subjects = new List<Subject>
            {
                new Subject
                {
                    Name = "Math"
                },
                new Subject
                {
                    Name = "History"
                },
            };

            context.Subjects.AddRange(subjects);

            context.SaveChanges();
        }

    }
}
