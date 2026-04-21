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

        /// <summary>
        /// Middleware encargado de capturar excepciones no controladas durante el pipeline HTTP
        /// </summary>
        /// <param name="pContext">Contexto HTTP de la solicitud actual</param>
        /// <returns>Tarea asincrónica que representa la ejecución del middleware</returns>
        /// <remarks>
        /// Este middleware intercepta cualquier excepción no manejada en la aplicación,
        /// registra el error utilizando el sistema de logging y retorna una respuesta
        /// estructurada en formato JSON con un código HTTP 500.
        ///
        /// Incluye un identificador único de traza (TraceId) que permite correlacionar
        /// errores en logs para facilitar el diagnóstico.
        /// </remarks>
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
