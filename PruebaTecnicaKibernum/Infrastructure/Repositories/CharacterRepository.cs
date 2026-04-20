using Microsoft.EntityFrameworkCore;
using PruebaTecnicaKibernum.Application.Interfaces;
using PruebaTecnicaKibernum.Domain.Entities;
using PruebaTecnicaKibernum.Infrastructure.DataContext;

namespace PruebaTecnicaKibernum.Infrastructure.Repositories
{
    public class CharacterRepository : ICharacterRepository
    {
        private readonly AppDbContext _Context;

        public CharacterRepository(AppDbContext Context)
        {
            _Context = Context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pExternalId"></param>
        /// <returns></returns>
        public async Task<bool> ExistsByExternalIdAsync(int pExternalId)
        {
            return await _Context.Character
                .AnyAsync(c => c.ExternalId == pExternalId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pCharacter"></param>
        /// <returns></returns>
        public async Task AddAsync(Character pCharacter)
        {
            await _Context.Character.AddAsync(pCharacter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task SaveChangesAsync()
        {
            await _Context.SaveChangesAsync();
        }
    }
}
