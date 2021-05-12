using Project.CrossCutting.Base;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Project.Api.Middleware
{
    public class ErrorCatcherMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorCatcherMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (DomainException ex)
            {
                if (httpContext.Response.HasStarted)
                {
                    throw;
                }

                httpContext.Response.Clear();
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                httpContext.Response.ContentType = @"application/json";
                httpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");

                await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(new { ex.Message }));
                return;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ErrorCatcherMiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorCatcherMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorCatcherMiddleware>();
        }
    }
}
