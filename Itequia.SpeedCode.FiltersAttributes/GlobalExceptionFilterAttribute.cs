using Itequia.SpeedCode.Entities.Exception;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;

namespace Itequia.SpeedCode.FiltersAttributes
{
    public class GlobalExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger<GlobalExceptionFilterAttribute> _logger;

        public GlobalExceptionFilterAttribute(ILogger<GlobalExceptionFilterAttribute> logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            ExceptionBase exception = null;
            if (context.Exception is ExceptionBase)
                exception = (ExceptionBase)context.Exception;
            else
            {
                exception = new ExceptionBase(context.Exception?.Message, context.Exception);
                exception.HttpStatus = System.Net.HttpStatusCode.InternalServerError;
            }
                


            var result = new ObjectResult(new
            {
                exception.Message,
                exception.Source,
                ExceptionType = exception.GetType().FullName,
                StatusCode = (int)(exception.HttpStatus),
                Stacktrace = exception.GetBaseException()?.StackTrace
            });


            _logger.Log((exception.LogLevel), "{reasonPhrase}: {ex}", exception.ReasonPhrase, context.Exception);
            
            context.HttpContext.Response.StatusCode = (int)exception.HttpStatus;
            context.Result = result;
        }
    }
}
