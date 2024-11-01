using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PMS.Data;
using PMS.Data.Models;
using PMS.Data.Models.Identity;
using PMS.Data.Repository;
using PMS.Data.Repository.Interfaces;
using PMS.Data.Seeders;
using PMS.Services.Data;
using PMS.Services.Data.Interfaces;

namespace PMSWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<PMSDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            builder.Services.AddDefaultIdentity<PMSUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<PMSDbContext>();
            builder.Services.AddControllersWithViews();

            /// Adding scoped services
            builder.Services.AddScoped<IRepository<Consumable, Guid>, GenericRepository<Consumable,Guid>>();
            builder.Services.AddScoped<IConsumableService, ConsumableService>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}
