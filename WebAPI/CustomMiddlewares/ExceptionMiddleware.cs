namespace WebAPI.CustomMiddlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _nextRequest;
        private readonly ILogger<ExceptionMiddleware> _logger;
        public ExceptionMiddleware(RequestDelegate nextRequest, ILogger<ExceptionMiddleware> logger)
        {
            _nextRequest = nextRequest;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context){
            try
            {
              await _nextRequest(context);  
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync(ex.Message);
            }
        }
    }
}