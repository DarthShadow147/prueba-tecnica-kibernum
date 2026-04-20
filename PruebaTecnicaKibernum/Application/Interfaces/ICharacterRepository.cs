using PruebaTecnicaKibernum.Domain.Entities;

namespace PruebaTecnicaKibernum.Application.Interfaces
{
    public interface ICharacterRepository
    {
        Task<bool> ExistsByExternalIdAsync(int pExternalId);
        Task AddAsync(Character pCharacter);
        Task SaveChangesAsync();
    }
}
