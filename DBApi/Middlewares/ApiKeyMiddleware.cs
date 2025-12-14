namespace DBApi.Middlewares
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _apiKey;
        private const string API_KEY_HEADER = "X-API-Key";

        // Các endpoint cần bảo vệ (yêu cầu API Key)
        private readonly string[] _protectedPaths = new[]
        {
            "/api/upload",      // POST - đăng bài
            "/api/posts/"       // DELETE, PUT - xóa/sửa bài
        };

        public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _apiKey = configuration["Security:ApiKey"] 
                ?? throw new Exception("API Key not configured in appsettings.json");
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower() ?? "";
            var method = context.Request.Method;

            // Kiểm tra xem có phải endpoint cần bảo vệ không
            bool needsProtection = false;

            // POST /api/upload - cần bảo vệ
            if (path.Contains("/api/upload") && method == "POST")
            {
                needsProtection = true;
            }
            // DELETE /api/posts/{id} - cần bảo vệ
            else if (path.Contains("/api/posts/") && method == "DELETE")
            {
                needsProtection = true;
            }
            // PUT /api/posts/{id} - cần bảo vệ
            else if (path.Contains("/api/posts/") && method == "PUT")
            {
                needsProtection = true;
            }

            // Nếu cần bảo vệ, kiểm tra API Key
            if (needsProtection)
            {
                if (!context.Request.Headers.TryGetValue(API_KEY_HEADER, out var providedKey))
                {
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync("{\"code\":401,\"message\":\"API Key is required\"}");
                    return;
                }

                if (!_apiKey.Equals(providedKey))
                {
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync("{\"code\":401,\"message\":\"Invalid API Key\"}");
                    return;
                }
            }

            // Tiếp tục xử lý request
            await _next(context);
        }
    }

    // Extension method để dễ sử dụng
    public static class ApiKeyMiddlewareExtensions
    {
        public static IApplicationBuilder UseApiKeyAuthentication(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiKeyMiddleware>();
        }
    }
}
