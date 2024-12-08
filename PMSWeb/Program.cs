using PMSWeb.Infrastructure.Extensions;

namespace PMSWeb
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Adding database only services - see extensions
            builder.Services.AddPMSDatabase(builder.Configuration);

            //Adding Identity services - see extensions
            builder.Services.AddPMSIdentity(builder.Configuration);

            //Additional services if any not related to main business logic - see extensions
            builder.Services.AddPMSServices(builder.Configuration);

            //Adding the repositories - see extensions
            builder.Services.RegisterRepositories();

            //Adding business related services - see extensions
            builder.Services.RegisterMyServices();

            //Adding controllers and views for ASP.Net - not customized
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseStatusCodePagesWithReExecute("/Home/StatusPageHandler", "?code={0}"); // Error handling 404 and 500 as requested. Can be moved to production env. when needed. 
                app.UseDeveloperExceptionPage();  // Only included for development environment. 
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            /// change the sheme to HTTPS for any incoming, thus avoid redirection in the next middleware app.UseHttpsRedirection()    
            //app.Use(async (context, next) =>
            //{
            //    context.Request.Scheme = "https";    

            //    await next();
            //});
            app.UseGoogleCloudCredentials();

            app.UseHttpsRedirection();  // if this on receives HTTP he will redirect to HTTPS !!!
            app.UseStaticFiles();       // app can work with static files .json .img etc

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(    // with map we can add branches to the pipeline and to send it to "other directions"
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            // Auto apply migrations at app start - see extensions
            app.ApplyMigrations();

            // Check and create roles for access levels if not already created at app start - see extensions
            await app.CreateRolesAsync();

            app.Run();           // delegate with run will close the pipeline
        }
    }
}
