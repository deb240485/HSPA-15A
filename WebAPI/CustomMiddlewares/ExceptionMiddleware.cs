using System.Net;
using WebAPI.Errors;

namespace WebAPI.CustomMiddlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _nextRequest;
        private readonly ILogger<ExceptionMiddleware> _logger;

        private readonly IHostEnvironment _env;
        public ExceptionMiddleware(RequestDelegate nextRequest, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _nextRequest = nextRequest;
            _logger = logger;
            _env = env;
        }

        public async Task Invoke(HttpContext context){
            try
            {
              await _nextRequest(context);  
            }
            catch (Exception ex)
            {
                ApiError respErrors;
                HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
                String message;
                var exceptionType = ex.GetType();

                if(exceptionType == typeof(UnauthorizedAccessException)){
                    statusCode = HttpStatusCode.Forbidden;
                    message = "You are not authorized";
                }else{
                    statusCode = HttpStatusCode.InternalServerError;
                    message = "Some unknown error occured";
                }

                if(_env.IsDevelopment()){
                    respErrors = new ApiError((int)statusCode, ex.Message, ex.StackTrace?.ToString());
                }else{
                    respErrors = new ApiError((int)statusCode,message);
                }
                
                _logger.LogError(ex, ex.Message);
                context.Response.StatusCode = (int)statusCode;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(respErrors.ToString()!);
            }
        }
    }
}