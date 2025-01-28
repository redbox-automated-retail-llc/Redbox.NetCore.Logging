using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Redbox.NetCore.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Redbox.NetCore.Logging.Filter
{
    internal class LoggingActionFilter : IAsyncActionFilter, IFilterMetadata
    {
        private readonly ILogger<LoggingActionFilter> _logger;
        private readonly IConfiguration _configuration;

        public LoggingActionFilter(ILogger<LoggingActionFilter> logger, IConfiguration configuration)
        {
            this._logger = logger;
            this._configuration = configuration;
        }

        public async Task OnActionExecutionAsync(
          ActionExecutingContext context,
          ActionExecutionDelegate next)
        {
            ControllerActionDescriptor actionDescriptor = (ControllerActionDescriptor)((ActionContext)context).ActionDescriptor;
            string controllerName = actionDescriptor.ControllerName;
            string actionName = actionDescriptor.ActionName;
            StringBuilder scrubbedRequest = new StringBuilder();
            context.ActionArguments.ToList<KeyValuePair<string, object>>().ForEach((Action<KeyValuePair<string, object>>)(arg =>
            {
                if (arg.Value is IMessageScrub)
                    scrubbedRequest.Append(string.Format(",{0}:{1}", (object)arg.Key, (arg.Value as IMessageScrub).Scrub()));
                else
                    scrubbedRequest.Append(string.Format(",{0}:{1}", (object)arg.Key, arg.Value));
            }));
            LoggerExtensions.LogInformation((ILogger)this._logger, string.Format("START: {0}.{1} {2}", (object)controllerName, (object)actionName, (object)scrubbedRequest), Array.Empty<object>());
            Stopwatch requestTimer = Stopwatch.StartNew();
            ActionExecutedContext actionExecutedContext = await next.Invoke();
            StringBuilder stringBuilder = new StringBuilder();
            if (actionExecutedContext?.Result != null)
            {
                if (actionExecutedContext.Result is OkObjectResult result3)
                {
                    if (result3.Value is IMessageScrub messageScrub1)
                        stringBuilder.Append(string.Format("{0}", messageScrub1.Scrub()));
                    else
                        stringBuilder.Append(string.Format("{0}", (object)result3.StatusCode));
                }
                else if (actionExecutedContext.Result is ContentResult result2)
                    stringBuilder.Append(result2.Content);
                else if (actionExecutedContext.Result is ObjectResult result1)
                {
                    if (result1.Value is IMessageScrub messageScrub2)
                        stringBuilder.Append(string.Format("{0}", messageScrub2.Scrub()));
                    else
                        stringBuilder.Append(string.Format("{0}", (object)result1.StatusCode));
                }
                int? nullable = (int?)actionExecutedContext.Result.GetType().GetProperty("StatusCode")?.GetValue((object)actionExecutedContext.Result);
                if (nullable.HasValue)
                    ((ActionContext)context).HttpContext.Response.StatusCode = nullable.Value;
            }
            else
                stringBuilder.Append("none");
            requestTimer.Stop();
            LoggerExtensions.LogInformation((ILogger)this._logger, string.Format("{0}.{1},response={2}", (object)controllerName, (object)actionName, (object)stringBuilder), Array.Empty<object>());
            LoggerExtensions.LogInformation((ILogger)this._logger, string.Format("END: {0}.{1},statuscode={2},elapsed={3}", (object)controllerName, (object)actionName, (object)(HttpStatusCode)((ActionContext)context).HttpContext.Response.StatusCode, (object)requestTimer.ElapsedMilliseconds), Array.Empty<object>());
            controllerName = (string)null;
            actionName = (string)null;
            requestTimer = (Stopwatch)null;
        }
    }
}
