using PruebaTecnicaKibernum.Application.Dtos.CharacterDto;
using PruebaTecnicaKibernum.Domain.Entities;

namespace PruebaTecnicaKibernum.Application.Interfaces
{
    public interface ICharacterService
    {
        Task ImportCharacterAsync();
        Task<PagedResult<CharacterResumeDto>> GetPagedAsync(CharacterQueryParameters pQuery);
        Task<CharacterResumeDto?> GetCharacterByIdAsync(int pId);
    }
}
