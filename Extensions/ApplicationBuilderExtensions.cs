using Microsoft.AspNetCore.Builder;
using Redbox.NetCore.Logging.Filter;
using System;

namespace Redbox.NetCore.Logging.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseMetricsActionFilterExtensions(
          this IApplicationBuilder builder)
        {
            return UseMiddlewareExtensions.UseMiddleware<LoggingActionFilter>(builder, Array.Empty<object>());
        }
    }
}
