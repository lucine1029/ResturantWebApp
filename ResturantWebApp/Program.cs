
//using RestaurantWebApp.Handlers;
using Microsoft.AspNetCore.Authentication.Cookies;
using RestaurantWebApp.Services;

namespace ResturantWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpContextAccessor();
            //builder.Services.AddTransient<AuthHeaderHandler>();

            builder.Services.AddHttpClient<IRestaurantApiService, RestaurantApiService>();

            //builder.Services.AddHttpClient<IRestaurantApiService, RestaurantApiService>()
            //    .AddHttpMessageHandler<AuthHeaderHandler>();

            // Add cookie authentication
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    //options.Cookie.Name = "auth_token";
                    //options.Cookie.HttpOnly = true;
                    //options.Cookie.SecurePolicy = CookieSecurePolicy.None; // For localhost
                    //options.Cookie.SameSite = SameSiteMode.Lax;
                    options.LoginPath = "/Admin/Login"; // redirect here if not logged in
                    options.LogoutPath = "/Admin/Logout";
                    //options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                    //options.SlidingExpiration = true;
                });



            // Add your custom API service
            builder.Services.AddScoped<RestaurantApiService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
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

            app.Run();
        }
    }
}
