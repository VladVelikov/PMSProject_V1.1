using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PMS.Data;
using PMS.Data.Models.Identity;
using PMSWeb.Infrastructure.Extensions;

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
            
            //IDENTITY SECTION
            builder.Services.AddDefaultIdentity<PMSUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<PMSDbContext>()
                .AddDefaultTokenProviders();   // add default token prov for e-mail change, usernamechange , confirm e-mail etc. 

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login";
                
                //options.LoginPath = "/Home/Dashboard";
                // add other configurations here
            });

            //builder.Services.AddAuthentication()
                

            //builder.Services.AddAuthorization(options => {
            //    options.AddPolicy("NeededClaim", policy => policy.RequireClaim("GoldenClaim"));
            //});
            
             
            //IDENTITY END

            builder.Services.AddControllersWithViews();

            builder.Services.RegisterRepositories();  /// Adding scoped services Repos via extension class method
            builder.Services.RegisterMyServices();    /// Adding scoped services Services via extension class method

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
