using PruebaTecnicaKibernum.Domain.Entities;

namespace PruebaTecnicaKibernum.Application.Interfaces
{
    public interface IHiringRequestRepository
    {
        Task AddAsync(HiringRequest pRequest);
        Task<List<HiringRequest>> GetAllAsync();
        Task<HiringRequest?> GetByIdAsync(int pId);
        Task SaveChangesAsync();
    }
}
