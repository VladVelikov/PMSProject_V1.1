using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PMS.Data;
using PMS.Data.Models.Identity;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IdentityServiceCollectionExtension
    {
        public static IServiceCollection AddPMSDatabase(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            services.AddDbContext<PMSDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddDatabaseDeveloperPageExceptionFilter();
            return services;
        }

        public static IServiceCollection AddPMSIdentity(this IServiceCollection services, IConfiguration config)
        {
            services.AddDefaultIdentity<PMSUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<PMSDbContext>()
                .AddDefaultTokenProviders();   // add default token prov for e-mail change, usernamechange , confirm e-mail etc. 

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.LogoutPath = "/Identity/Account/Logout";
                options.Cookie.HttpOnly = true;
            });

            //builder.Services.AddAuthentication() // TODO Later


            //builder.Services.AddAuthorization(options => {
            //    options.AddPolicy("NeededClaim", policy => policy.RequireClaim("GoldenClaim"));
            //});

            return services;
        }

        public static IServiceCollection AddPMSServices(this IServiceCollection services, IConfiguration config)
        {

            return services;
        }


    }
}
