//using Microsoft.AspNetCore.Http;
//using System.Net.Http.Headers;

//namespace RestaurantWebApp.Handlers
//{
//    public class AuthHeaderHandler : DelegatingHandler
//    {
//        private readonly IHttpContextAccessor _httpContextAccessor;

//        public AuthHeaderHandler(IHttpContextAccessor httpContextAccessor)
//        {
//            _httpContextAccessor = httpContextAccessor;
//        }

//        protected override async Task<HttpResponseMessage> SendAsync(
//            HttpRequestMessage request, CancellationToken cancellationToken)
//        {
//            //var token = _httpContextAccessor.HttpContext?.Request.Cookies["auth_token"];

//            //if (!string.IsNullOrEmpty(token))
//            //{
//            //    request.Headers.Authorization =
//            //        new AuthenticationHeaderValue("Bearer", token);
//            //}

//            //return await base.SendAsync(request, cancellationToken);


//            var token = _httpContextAccessor.HttpContext?.Request.Cookies["auth_token"];

//            Console.WriteLine($"=== AUTH HEADER HANDLER ===");
//            Console.WriteLine($"Request: {request.Method} {request.RequestUri}");
//            Console.WriteLine($"Token found: {!string.IsNullOrEmpty(token)}");

//            if (!string.IsNullOrEmpty(token))
//            {
//                // Clean the token
//                token = token.Trim();
//                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
//                Console.WriteLine($"✅ Added Bearer token to request");
//            }
//            else
//            {
//                Console.WriteLine($"❌ No token available");
//            }

//            var response = await base.SendAsync(request, cancellationToken);

//            Console.WriteLine($"Response Status: {response.StatusCode}");
//            Console.WriteLine($"=== END AUTH HANDLER ===");

//            return response;



//        }
//    }
//}
