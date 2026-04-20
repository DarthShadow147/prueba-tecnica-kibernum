using Microsoft.AspNetCore.Mvc;
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
    }
}
