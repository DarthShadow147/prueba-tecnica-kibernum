using Microsoft.EntityFrameworkCore;
using PruebaTecnicaKibernum.Application.Dtos.HiringDto;
using PruebaTecnicaKibernum.Application.Interfaces;
using PruebaTecnicaKibernum.Domain.Entities;
using PruebaTecnicaKibernum.Domain.Enums;
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
        /// <param name="pQuery"></param>
        /// <returns></returns>
        public async Task<List<HiringRequest>> GetFilteredAsync(HiringRequestQueryParams pQuery)
        {
            var lDbQuery = _Context.HiringRequest
                .Include(x => x.Character)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pQuery.Status) && Enum.TryParse<RequestStatus>(pQuery.Status, true, out var status))
                lDbQuery = lDbQuery.Where(x => x.Status == status);

            if (!string.IsNullOrWhiteSpace(pQuery.Applicant))
                lDbQuery = lDbQuery.Where(x => x.Applicant.Contains(pQuery.Applicant));

            return await lDbQuery
                .OrderByDescending(x => x.CreatedAt)
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
                .Include(x => x.Character)
                .FirstOrDefaultAsync(x => x.Id == pId);
        }

        public async Task<List<HiringRequest>> GetAllWithCharacterAsync()
        {
            return await _Context.HiringRequest
                .Include(x => x.Character)
                .ToListAsync();
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
