using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PMS.Data;
using static PMS.Common.EntityValidationConstants;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope serviceScope = app.ApplicationServices.CreateScope();
            PMSDbContext context = serviceScope.ServiceProvider.GetService<PMSDbContext>()!;
            context.Database.Migrate();
            return app;

        }

        public static async Task<IApplicationBuilder> CreateRolesAsync(this IApplicationBuilder app)
        {
               using IServiceScope scope = app.ApplicationServices.CreateScope(); 
               RoleManager<IdentityRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
               var roles = PMSPositions;
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

            return app; 
        }
    }
}
