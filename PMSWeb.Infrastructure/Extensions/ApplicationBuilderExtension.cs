using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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

        public static IApplicationBuilder UseGoogleCloudCredentials(this IApplicationBuilder app)
        {
            var env = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();

            string credentialsPath = Path.Combine(env.ContentRootPath, "wwwroot", "light-processor-442809-u0-1b64b7eda3d4.json");

            if (!File.Exists(credentialsPath))
            {
                throw new FileNotFoundException("Google Cloud credentials file not found.", credentialsPath);
            }

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialsPath);

            Console.WriteLine($"Google Cloud credentials set from: {credentialsPath}");

            return app;
        }

    }
}
