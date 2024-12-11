using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace VkWeb.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetRawTarget(this HttpRequest request)
        {
            var httpRequestFeature = request.HttpContext.Features.Get<IHttpRequestFeature>(); 
            return httpRequestFeature.RawTarget;
        }

    }
}
