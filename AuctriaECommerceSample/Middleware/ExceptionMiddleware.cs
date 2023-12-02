using AuctriaECommerceSample.Models;
using Serilog;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace AuctriaECommerceSample.Middleware
{

    public class ExceptionMiddleware
    {
        private readonly Serilog.ILogger _logger;
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next)
        {
            _logger = Log.Logger;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (CustomValidationException exValid)
            {
                _logger.Error(exValid.Message + "\r\nStackTrace:\r\n" + exValid.StackTrace, "An Unhandled exception occured.");
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsync(exValid.Message);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occured while executing the operation");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsync(ex.Message);
            }

        }

    }
}
