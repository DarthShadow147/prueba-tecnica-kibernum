using Microsoft.EntityFrameworkCore;
using PruebaTecnicaKibernum.Application.Interfaces;
using PruebaTecnicaKibernum.Domain.Entities;
using PruebaTecnicaKibernum.Infrastructure.DataContext;

namespace PruebaTecnicaKibernum.Infrastructure.Repositories
{
    public class HiringRequestRepository : IHiringRequestRepository
    {
        private readonly AppDbContext _Context;

        public HiringRequestRepository(AppDbContext Context)
        {
            _Context = Context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns></returns>
        public async Task AddAsync(HiringRequest pRequest)
        {
            await _Context.HiringRequest.AddAsync(pRequest);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<HiringRequest>> GetAllAsync()
        {
            return await _Context.HiringRequest
                .Include(x => x.Character)
                .ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pId"></param>
        /// <returns></returns>
        public async Task<HiringRequest?> GetByIdAsync(int pId)
        {
            return await _Context.HiringRequest
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
