using Microsoft.AspNetCore.Http;

namespace VkWeb.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetBaseUrl(this IHttpContextAccessor context)
        {
            return $"{context.HttpContext.Request.Scheme}://{context.HttpContext.Request.Host.ToUriComponent()}";
        }
    }
}
