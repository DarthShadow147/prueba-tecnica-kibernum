using Microsoft.EntityFrameworkCore;
using PruebaTecnicaKibernum.Application.Dtos.CharacterDto;
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
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<(List<Character> Data, int TotalCount)> GetPagedAsync(CharacterQueryParameters pQuery)
        {
            var lDbQuery = _Context.Character.AsQueryable();

            if (!string.IsNullOrWhiteSpace(pQuery.Name))
                lDbQuery = lDbQuery.Where(x => x.Name.Contains(pQuery.Name));

            if (!string.IsNullOrWhiteSpace(pQuery.Status))
                lDbQuery = lDbQuery.Where(x => x.Status == pQuery.Status);

            var lTotalCount = await lDbQuery.CountAsync();

            var lData = await lDbQuery
                .Skip((pQuery.Page - 1) * pQuery.PageSize)
                .Take(pQuery.PageSize)
                .ToListAsync();

            return (lData, lTotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pId"></param>
        /// <returns></returns>
        public async Task<Character?> GetByIdAsync(int pId)
        {
            return await _Context.Character
                .FirstOrDefaultAsync(x => x.Id == pId);
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
