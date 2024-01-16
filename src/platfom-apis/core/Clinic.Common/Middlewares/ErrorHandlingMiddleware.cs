using Clinic.Common.Models;
using Clinic.Common.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Clinic.Common.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate requestDelegate;
        private readonly ILogger logger;
        private readonly bool includeExceptionDetailsResponse;
        public ErrorHandlingMiddleware(RequestDelegate requestDelegate, ILogger<ErrorHandlingMiddleware> logger,
            IOptions<ApplicationSettings> applicationSettings)
        {
            this.requestDelegate = requestDelegate;
            this.logger = logger;
            this.includeExceptionDetailsResponse = applicationSettings.Value.IncludeExceptionStackInResponse;
             
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                if(this.requestDelegate != null)
                {
                    await this.requestDelegate.Invoke(context).ConfigureAwait(false);
                }
            }catch(Exception innerException)
            {
                logger.LogCritical(Constants.Constants.ErrorHandlingMiddlewareErrorCode, innerException, Constants.Constants.ErrorMiddlewareLog);
                ExceptionResponse currentException = new()
                {
                    ErrorMessage = Constants.Constants.ErrorMiddlewareLog,
                    CorrelationIdentifer = System.Diagnostics.Activity.Current?.RootId
                };
                if(this.includeExceptionDetailsResponse)
                {
                    currentException.InnerException = $"{innerException.Message} {innerException.StackTrace}";
                }
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize<ExceptionResponse>(currentException)).ConfigureAwait(false);
            }
        }
    }
}
