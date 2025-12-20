namespace DBApi.Middlewares
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _apiKey;
        private const string API_KEY_HEADER = "X-API-Key";

        private readonly string[] _protectedPaths = new[]
        {
            "/api/upload",
            "/api/posts/" 
        };

        public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _apiKey = Environment.GetEnvironmentVariable("API_KEY") 
                ?? configuration["Security:ApiKey"] 
                ?? throw new Exception("API Key not configured. Set environment variable API_KEY or configure in appsettings.json");
            
            if (string.IsNullOrEmpty(_apiKey))
            {
                throw new Exception("API Key cannot be empty. Set environment variable API_KEY");
            }
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower() ?? "";
            var method = context.Request.Method;
            bool needsProtection = false;

            if (path.Contains("/api/upload") && method == "POST") needsProtection = true;
            else if (path.Contains("/api/posts/") && method == "DELETE") needsProtection = true;
            else if (path.Contains("/api/posts/") && method == "PUT") needsProtection = true;

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

            await _next(context);
        }
    }

    public static class ApiKeyMiddlewareExtensions
    {
        public static IApplicationBuilder UseApiKeyAuthentication(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiKeyMiddleware>();
        }
    }
}
