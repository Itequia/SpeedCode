using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Itequia.SpeedCode.Security.Middleware
{
    public class CustomHeadersMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly IConfiguration _configuration;

        public object Configuration { get; private set; }

        public CustomHeadersMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.Headers.Add("versionapp", _configuration["Versionapp"]);
            await _next(context);
        }
    }
}
