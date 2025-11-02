using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using RestaurantWebApp.Models;
using RestaurantWebApp.Services;
using System.Security.Claims;

namespace RestaurantWebApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly IRestaurantApiService _restaurantApiService;
        public AdminController(IRestaurantApiService restaurantApiService)
        {
            _restaurantApiService = restaurantApiService;
        }

        [HttpGet]
        public IActionResult Index() // Display the Admin page
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login() // Display the login form
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)  // Handle login form submission and authentication and JWT storage in cookies
        {
            if (!ModelState.IsValid)
                return View(model);

            // call your API's /auth/login
            var token = await _restaurantApiService.LoginAsync(model);

            if (token == null)
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View(model);
            }

            // store JWT in cookie claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Username),
                new Claim("JWT", token)
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            // Save JWT token in a separate cookie for API calls
            Response.Cookies.Append("JWToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // set to true if using HTTPS
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(1)
            });

            return RedirectToAction("Index", "Admin");

            //        if (!ModelState.IsValid)
            //            return View(model);

            //        var token = await _restaurantApiService.LoginAsync(model);

            //        if (string.IsNullOrEmpty(token))
            //        {
            //            ModelState.AddModelError("", "Invalid username or password");
            //            return View(model);
            //        }

            //        // Clean the token
            //        token = token.Trim('"').Trim();
            //        Console.WriteLine($"=== STORING TOKEN ===");
            //        Console.WriteLine($"New token: {token.Substring(0, 50)}...");

            //        // ✅ NUCLEAR OPTION: Delete ALL possible auth cookies
            //        var cookieNames = new[] { "auth_token", "jwt_token", "debug_token", ".AspNetCore.Cookies" };
            //        foreach (var cookieName in cookieNames)
            //        {
            //            Response.Cookies.Delete(cookieName);
            //        }

            //        // ✅ Wait a moment for deletion to process
            //        await Task.Delay(100);

            //        // ✅ Store with DIFFERENT cookie name to avoid caching
            //        var timestamp = DateTime.Now.Ticks;
            //        var uniqueCookieName = $"auth_token_{timestamp}";

            //        var cookieOptions = new CookieOptions
            //        {
            //            HttpOnly = true,
            //            Secure = false,
            //            SameSite = SameSiteMode.Lax,
            //            Expires = DateTimeOffset.UtcNow.AddHours(24),
            //            Path = "/"
            //        };

            //        Response.Cookies.Append(uniqueCookieName, token, cookieOptions);
            //        Response.Cookies.Append("auth_token", token, cookieOptions); // Also set the standard name

            //        // ✅ VERIFY storage
            //        Console.WriteLine("Cookie verification:");
            //        Console.WriteLine($"- Unique cookie: {Request.Cookies[uniqueCookieName]?.Substring(0, 20)}...");
            //        Console.WriteLine($"- Standard cookie: {Request.Cookies["auth_token"]?.Substring(0, 20)}...");

            //        // ✅ Use claims as PRIMARY source (more reliable)
            //        var claims = new List<Claim>
            //{
            //    new Claim(ClaimTypes.Name, model.Username),
            //    new Claim("JWT", token), // This is the MOST important
            //    new Claim(ClaimTypes.Role, "SuperAdmin"),
            //    new Claim("AuthToken", token) // Backup claim
            //};

            //        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            //        var principal = new ClaimsPrincipal(identity);

            //        await HttpContext.SignInAsync(
            //            CookieAuthenticationDefaults.AuthenticationScheme,
            //            principal,
            //            new AuthenticationProperties
            //            {
            //                IsPersistent = true,
            //                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(24)
            //            });

            //        Console.WriteLine($"=== LOGIN COMPLETE ===");
            //        return RedirectToAction("Index", "Admin");



        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
