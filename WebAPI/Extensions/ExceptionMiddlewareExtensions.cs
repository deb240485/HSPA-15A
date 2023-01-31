using System.Net;
using Microsoft.AspNetCore.Diagnostics;

namespace WebAPI.Extensions
{
    //// handling exception globally through extension method.
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app){
            app.UseExceptionHandler(options => {
                options.Run(
                    async context => {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    var ex = context.Features.Get<IExceptionHandlerFeature>();
                    if(ex != null){
                    await context.Response.WriteAsync(ex.Error.Message);  
                    }
                  }
                );
           });
        }
    }
}