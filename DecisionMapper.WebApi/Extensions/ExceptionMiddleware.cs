using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DecisionMapper.WebApi.Extensions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate action;

        public ExceptionMiddleware(RequestDelegate action)
        {
            this.action = action;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await action(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            if ((exception is ValidationException) || (exception is ArgumentException))
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return context.Response.WriteAsync("Incorrect incoming data");
            }

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync("Internal Server Error");
        }
    }
}