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

        [HttpPost("import")]
        public async Task<IActionResult> Import(CancellationToken cancellationToken)
        {
            _Logger.LogInformation("Starting character import");
            await _CharacterService.ImportCharacterAsync();
            _Logger.LogInformation("Character import completed");

            return Ok(new { message = "Characters imported successfully" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] CharacterQueryParameters pQuery)
        {
            var lCharacterData = await _CharacterService.GetPagedAsync(pQuery);
            return Ok(lCharacterData);
        }

        [HttpGet("{pId}")]
        public async Task<IActionResult> GetById(int pId)
        {
            var lCharacterData = await _CharacterService.GetCharacterByIdAsync(pId);
            return Ok(lCharacterData);
        }
    }
}
