using PruebaTecnicaKibernum.Application.Dtos.HiringDto;
using PruebaTecnicaKibernum.Domain.Enums;

namespace PruebaTecnicaKibernum.Application.Interfaces
{
    public interface IHiringRequestService
    {
        Task<int> CreateAsync(CreateHiringRequestDto pRequest);
        Task<IEnumerable<HiringRequestDto>> GetAllAsync();
        Task UpdateStatusAsync(int pId, RequestStatus pStatus);
    }
}
