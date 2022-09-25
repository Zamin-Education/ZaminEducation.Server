using ZaminEducation.Service.Exceptions;

namespace ZaminEducation.Api.Middlewares
{
    public class ZaminEducationExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ZaminEducationExceptionMiddleware> logger;
        public ZaminEducationExceptionMiddleware(RequestDelegate next, ILogger<ZaminEducationExceptionMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await this.next.Invoke(context);
            }
            catch (ZaminEducationException ex)
            {
                await this.HandleException(context, ex.Code, ex.Message);
            }
            catch (Exception ex)
            {
                //Log
                logger.LogError(ex.ToString());
                
                await this.HandleException(context, 500, ex.Message);
            }
        }

        public async Task HandleException(HttpContext context, int code, string message)
        {
            context.Response.StatusCode = code;
            await context.Response.WriteAsJsonAsync(new
            {
                Code = code,
                Message = message
            });
        }
    }
}
