using Microsoft.AspNetCore.Identity;

namespace VideoGameForum.Data
{
    public static class ApplicationBuilderExtensions
    {
        public static async Task EnsureRolesCreated(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roleNames = { "Admin", "User" };  

            foreach (var roleName in roleNames)
            {
                bool roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
    }
}
