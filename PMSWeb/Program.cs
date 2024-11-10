using Microsoft.EntityFrameworkCore;
using PMS.Data;
using PMS.Data.Models;
using PMS.Data.Models.Identity;
using PMS.Data.Repository;
using PMS.Data.Repository.Interfaces;
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

            /// Adding scoped services Repos
            builder.Services.AddScoped<IRepository<Consumable, Guid>, GenericRepository<Consumable,Guid>>();
            builder.Services.AddScoped<IRepository<City, Guid>, GenericRepository<City, Guid>>();
            builder.Services.AddScoped<IRepository<Country, Guid>, GenericRepository<Country, Guid>>();
            builder.Services.AddScoped<IRepository<Equipment, Guid>, GenericRepository<Equipment, Guid>>();
            builder.Services.AddScoped<IRepository<Maker, Guid>, GenericRepository<Maker, Guid>>();
            builder.Services.AddScoped<IRepository<Manual, Guid>, GenericRepository<Manual, Guid>>();
            builder.Services.AddScoped<IRepository<RoutineMaintenance, Guid>, GenericRepository<RoutineMaintenance, Guid>>();
            builder.Services.AddScoped<IRepository<Sparepart, Guid>, GenericRepository<Sparepart, Guid>>();
            builder.Services.AddScoped<IRepository<SpecificMaintenance, Guid>, GenericRepository<SpecificMaintenance, Guid>>();
            builder.Services.AddScoped<IRepository<Supplier, Guid>, GenericRepository<Supplier, Guid>>();
            builder.Services.AddScoped<IRepository<ConsumableEquipment, Guid[]>, GenericRepository<ConsumableEquipment, Guid[]>>();
            builder.Services.AddScoped<IRepository<RoutineMaintenanceEquipment, Guid[]>, GenericRepository<RoutineMaintenanceEquipment, Guid[]>>();
            builder.Services.AddScoped<IRepository<ConsumableSupplier, Guid[]>, GenericRepository<ConsumableSupplier, Guid[]>>();
            builder.Services.AddScoped<IRepository<SparepartSupplier, Guid[]>, GenericRepository<SparepartSupplier, Guid[]>>();

            
            /// Adding scoped services Services
            builder.Services.AddScoped<IConsumableService, ConsumableService>();
            builder.Services.AddScoped<ICityService, CityService>();
            builder.Services.AddScoped<ICountryService, CountryService>();
            builder.Services.AddScoped<IEquipmentService, EquipmentService>();
            builder.Services.AddScoped<IMakerService, MakerService>();
            builder.Services.AddScoped<IManualService, ManualService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();  // To be removed for production environment 
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // app.UseStatusCodePagesWithReExecute(); TO DO
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            /// change the sheme to HTTPS for any incoming, thus avoid redirection in the next middleware app.UseHttpsRedirection()
            //app.Use(async (context, next) =>
            //{
            //    context.Request.Scheme = "https";    

            //    await next();
            //});



            app.UseHttpsRedirection();  // if this on receives HTTP he will redirect to HTTPS !!!
            app.UseStaticFiles();       // app can work with static files .json .img etc

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(                // with map we can add branches to the pipeline and to send it to "other directions"
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();           // delegate with run will close the pipeline
        }
    }
}
