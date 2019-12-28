using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;

namespace Walterlv.Http2Https
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.Use(async (context, next) =>
            {
                if (!context.Request.IsHttps
                    && !string.Equals("localhost", context.Request.Host.Host, StringComparison.OrdinalIgnoreCase)
                    && !string.Equals("127.0.0.1", context.Request.Host.Host, StringComparison.OrdinalIgnoreCase)
                    && !string.Equals("[::1]", context.Request.Host.Host, StringComparison.OrdinalIgnoreCase))
                {
                    var url = "https://" + context.Request.Host + context.Request.PathBase + context.Request.Path;
                    context.Response.Redirect(url);
                    context.Response.StatusCode = 301;
                    return;
                }
                await next().ConfigureAwait(false);
            });
        }
    }
}
