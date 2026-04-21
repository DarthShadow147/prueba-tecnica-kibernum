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

        /// <summary>
        /// Crea una nueva solicitud de contratación de un personaje
        /// </summary>
        /// <param name="pRequest">Datos de la solicitud (personaje, solicitante, evento y fecha)</param>
        /// <param name="pValidator">Reglas de validacion de campos obligatorios</param>
        /// <returns>Identificador de la solicitud creada</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateHiringRequestDto pRequest, [FromServices] IValidator<CreateHiringRequestDto> pValidator)
        {
            var lValidationResult = await pValidator.ValidateAsync(pRequest);
            if (!lValidationResult.IsValid)
                return BadRequest(lValidationResult.Errors);

            var lTransaction = await _Service.CreateAsync(pRequest);
            return Ok(lTransaction);
        }

        /// <summary>
        /// Obtiene el listado de solicitudes con filtros opcionales
        /// </summary>
        /// <param name="pQuery">Filtros por estado o solicitante</param>
        /// <returns>Listado de solicitudes</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] HiringRequestQueryParams pQuery)
        {
            var lHiringData = await _Service.GetFilteredAsync(pQuery);
            return Ok(lHiringData);
        }

        /// <summary>
        /// Obtiene el detalle de una solicitud específica
        /// </summary>
        /// <param name="pId">Identificador de la solicitud</param>
        /// <returns>Información detallada de la solicitud</returns>
        [HttpGet("{pId}")]
        public async Task<IActionResult> GetById(int pId)
        {
            var lHiringData = await _Service.GetByIdAsync(pId);
            return Ok(lHiringData);
        }

        /// <summary>
        /// Actualiza el estado de una solicitud
        /// </summary>
        /// <param name="pId">Identificador de la solicitud</param>
        /// <param name="status">Nuevo estado (Pending, InProgress, Approved, Rejected)</param>
        [HttpPatch("{pId}/status")]
        public async Task<IActionResult> UpdateStatus(int pId, [FromQuery] RequestStatus status)
        {
            await _Service.UpdateStatusAsync(pId, status);
            return NoContent();
        }
    }
}
