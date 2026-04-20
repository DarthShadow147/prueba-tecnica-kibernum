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

        [HttpGet]
        public async Task<IActionResult> GetSummary()
        {
            var lSummaryResult = await _Service.GetSummaryAsync();
            return Ok(lSummaryResult);
        }
    }
}
