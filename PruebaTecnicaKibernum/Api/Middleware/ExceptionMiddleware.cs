using PruebaTecnicaKibernum.Api.Models;

namespace PruebaTecnicaKibernum.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _Next;
        private readonly ILogger<ExceptionMiddleware> _Logger;

        public ExceptionMiddleware(RequestDelegate Next, ILogger<ExceptionMiddleware> Logger)
        {
            _Next = Next;
            _Logger = Logger;
        }
        
        public async Task Invoke(HttpContext pContext)
        {
            try
            {
                await _Next(pContext);
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Unhandled exception");
                pContext.Response.ContentType = "application/json";
                pContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

                var response = new ErrorResponse
                {
                    Message = "An unexpected error occurred",
                    TraceId = pContext.TraceIdentifier
                };

                await pContext.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
