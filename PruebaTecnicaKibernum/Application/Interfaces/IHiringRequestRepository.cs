using PruebaTecnicaKibernum.Application.Dtos.HiringDto;
using PruebaTecnicaKibernum.Domain.Entities;

namespace PruebaTecnicaKibernum.Application.Interfaces
{
    public interface IHiringRequestRepository
    {
        Task AddAsync(HiringRequest pRequest);
        Task<List<HiringRequest>> GetFilteredAsync(HiringRequestQueryParams pQuery);
        Task<HiringRequest?> GetByIdAsync(int pId);
        Task SaveChangesAsync();
    }
}
