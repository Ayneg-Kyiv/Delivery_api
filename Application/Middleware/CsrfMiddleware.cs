using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Application.Middleware
{
    public class CsrfMiddleware(RequestDelegate next, IAntiforgery antiforgery)
    {
        private readonly RequestDelegate _next = next;
        private readonly IAntiforgery _antiforgery = antiforgery;

        public async Task InvokeAsync(HttpContext context)
        {
            // Exclude SignalR endpoints from CSRF validation
            if (context.Request.Path.StartsWithSegments("/messagingHub"))
            {
                await _next(context);
                return;
            }

            // Exclude Google authentication endpoint from CSRF validation
            if (context.Request.Path.Equals("/api/auth/google-authenticate", StringComparison.OrdinalIgnoreCase))
            {
                await _next(context);
                return;
            }


            if (HttpMethods.IsGet(context.Request.Method) ||
                HttpMethods.IsHead(context.Request.Method) ||
                HttpMethods.IsOptions(context.Request.Method))
            {
                var tokens = _antiforgery.GetAndStoreTokens(context);

                context.Response.Cookies.Append(
                    "XSRF-TOKEN",
                    tokens.RequestToken!,
                    new CookieOptions
                    {
                        SameSite = SameSiteMode.None,
                        Secure = true,
                        HttpOnly = false
                    });
            }
            else if (HttpMethods.IsPost(context.Request.Method) ||
                     HttpMethods.IsPut(context.Request.Method) ||
                     HttpMethods.IsDelete(context.Request.Method) ||
                     HttpMethods.IsPatch(context.Request.Method))
            {
                // Validate the CSRF token for non-GET requests
                try
                {
                    await _antiforgery.ValidateRequestAsync(context);
                }
                catch (AntiforgeryValidationException)
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    return;
                }
            }

            // Call the next middleware in the pipeline
            await _next(context);
        }
    }

    // Extension method to use the middleware
    public static class CsrfMiddlewareExtensions
    {
        public static IApplicationBuilder UseCsrfProtection(this IApplicationBuilder app)
            => app.UseMiddleware<CsrfMiddleware>();
    }
}
