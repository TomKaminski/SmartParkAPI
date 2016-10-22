using System;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SmartParkAPI.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class AiHandleErrorAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext?.HttpContext != null && filterContext.Exception != null)
            {
                // Uwaga: jedno wystąpienie klienta telemetrii wystarczy do śledzenia wielu elementów telemetrii.
                var ai = new TelemetryClient();
                ai.TrackException(filterContext.Exception);
            }
            base.OnException(filterContext);
        }
    }
}