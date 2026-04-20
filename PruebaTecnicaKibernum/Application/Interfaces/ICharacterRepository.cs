using PruebaTecnicaKibernum.Application.Dtos.CharacterDto;
using PruebaTecnicaKibernum.Domain.Entities;

namespace PruebaTecnicaKibernum.Application.Interfaces
{
    public interface ICharacterRepository
    {
        Task<bool> ExistsByExternalIdAsync(int pExternalId);
        Task AddAsync(Character pCharacter);
        Task<(List<Character> Data, int TotalCount)> GetPagedAsync(CharacterQueryParameters pQuery);
        Task<Character?> GetByIdAsync(int pId);
        Task SaveChangesAsync();
    }
}
