using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using WebAPI.CustomMiddlewares;

namespace WebAPI.Extensions
{
    //// handling exception globally through extension method.
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureCustomExceptionHandler(this IApplicationBuilder app){
            
            app.UseMiddleware<ExceptionMiddleware>();
        }
        public static void ConfigureBuiltinExceptionHandler(this IApplicationBuilder app){
            
            // if(env.IsDevelopment())
            // {
            //     app.UseDeveloperExceptionPage();
            // }else{

                app.UseExceptionHandler(options => {
                options.Run(
                    async context => {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        var ex = context.Features.Get<IExceptionHandlerFeature>();
                        if(ex != null){
                        await context.Response.WriteAsync(ex.Error.Message);  
                        }
                    });
                });

            // }            
        }
    }
}