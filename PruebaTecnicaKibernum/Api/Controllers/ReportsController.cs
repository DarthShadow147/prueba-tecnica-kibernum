using Microsoft.AspNetCore.Mvc;
using PruebaTecnicaKibernum.Application.Interfaces;

namespace PruebaTecnicaKibernum.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IHiringRequestService _Service;

        public ReportsController(IHiringRequestService Service)
        {
            _Service = Service;
        }

        /// <summary>
        /// Obtiene un resumen de solicitudes
        /// </summary>
        /// <returns>Totales por estado y el personaje más solicitado</returns>
        [HttpGet]
        public async Task<IActionResult> GetSummary()
        {
            var lSummaryResult = await _Service.GetSummaryAsync();
            return Ok(lSummaryResult);
        }
    }
}
