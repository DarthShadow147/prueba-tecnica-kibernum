using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PruebaTecnicaKibernum.Application.Dtos.HiringDto;
using PruebaTecnicaKibernum.Application.Interfaces;
using PruebaTecnicaKibernum.Domain.Enums;

namespace PruebaTecnicaKibernum.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HiringController : ControllerBase
    {
        private readonly IHiringRequestService _Service;
        private readonly ILogger<HiringController> _Logger;

        public HiringController(IHiringRequestService Service, ILogger<HiringController> Logger)
        {
            _Service = Service;
            _Logger = Logger;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateHiringRequestDto pRequest, [FromServices] IValidator<CreateHiringRequestDto> pValidator)
        {
            var lValidationResult = await pValidator.ValidateAsync(pRequest);
            if (!lValidationResult.IsValid)
                return BadRequest(lValidationResult.Errors);

            var lTransaction = await _Service.CreateAsync(pRequest);
            return Ok(lTransaction);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] HiringRequestQueryParams pQuery)
        {
            var lHiringData = await _Service.GetFilteredAsync(pQuery);
            return Ok(lHiringData);
        }

        [HttpGet("{pId}")]
        public async Task<IActionResult> GetById(int pId)
        {
            var lHiringData = await _Service.GetByIdAsync(pId);
            return Ok(lHiringData);
        }

        [HttpPatch("{pId}/status")]
        public async Task<IActionResult> UpdateStatus(int pId, [FromQuery] RequestStatus status)
        {
            await _Service.UpdateStatusAsync(pId, status);
            return NoContent();
        }
    }
}
