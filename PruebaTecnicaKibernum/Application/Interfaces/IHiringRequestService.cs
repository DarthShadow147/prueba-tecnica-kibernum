using PruebaTecnicaKibernum.Application.Dtos.HiringDto;
using PruebaTecnicaKibernum.Application.Dtos.SummaryDto;
using PruebaTecnicaKibernum.Domain.Enums;

namespace PruebaTecnicaKibernum.Application.Interfaces
{
    public interface IHiringRequestService
    {
        Task<int> CreateAsync(CreateHiringRequestDto pRequest);
        Task<IEnumerable<HiringRequestDto>> GetFilteredAsync(HiringRequestQueryParams pQuery);
        Task<HiringRequestDto> GetByIdAsync(int pId);
        Task<HiringSummaryDto> GetSummaryAsync();
        Task UpdateStatusAsync(int pId, RequestStatus pStatus);
    }
}
