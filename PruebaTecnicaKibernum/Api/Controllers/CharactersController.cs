using Microsoft.AspNetCore.Mvc;
using PruebaTecnicaKibernum.Application.Dtos.CharacterDto;
using PruebaTecnicaKibernum.Application.Interfaces;

namespace PruebaTecnicaKibernum.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharactersController : ControllerBase
    {
        private readonly ICharacterService _CharacterService;
        private readonly ILogger<CharactersController> _Logger;

        public CharactersController(ICharacterService CharacterService, ILogger<CharactersController> Logger)
        {
            _CharacterService = CharacterService;
            _Logger = Logger;
        }

        /// <summary>
        /// Importa personajes desde la API externa de Rick & Morty y los guarda en la base de datos local
        /// </summary>
        /// <remarks>
        /// Evita duplicados utilizando el ExternalId del personaje.
        /// Puede tardar varios segundos dependiendo de la cantidad de páginas.
        /// </remarks>
        [HttpPost("import")]
        public async Task<IActionResult> Import()
        {
            _Logger.LogInformation("Starting character import");
            await _CharacterService.ImportCharacterAsync();
            _Logger.LogInformation("Character import completed");

            return Ok(new { message = "Characters imported successfully" });
        }

        /// <summary>
        /// Obtiene un listado paginado de personajes
        /// </summary>
        /// <param name="pQuery">Filtros opcionales: nombre, estado y paginación</param>
        /// <returns>Listado de personajes con paginación</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] CharacterQueryParameters pQuery)
        {
            var lCharacterData = await _CharacterService.GetPagedAsync(pQuery);
            return Ok(lCharacterData);
        }

        /// <summary>
        /// Obtiene el detalle de un personaje específico por su identificador
        /// </summary>
        /// <param name="pId">Identificador del personaje</param>
        /// <returns>Información del personaje</returns>
        [HttpGet("{pId}")]
        public async Task<IActionResult> GetById(int pId)
        {
            var lCharacterData = await _CharacterService.GetCharacterByIdAsync(pId);
            return Ok(lCharacterData);
        }
    }
}
