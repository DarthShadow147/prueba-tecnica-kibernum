using PruebaTecnicaKibernum.Application.Dtos.HiringDto;
using PruebaTecnicaKibernum.Application.Dtos.SummaryDto;
using PruebaTecnicaKibernum.Application.Interfaces;
using PruebaTecnicaKibernum.Domain.Entities;
using PruebaTecnicaKibernum.Domain.Enums;

namespace PruebaTecnicaKibernum.Application.Services
{
    public class HiringRequestService : IHiringRequestService
    {
        private readonly ICharacterRepository _CharacterRepository;
        private readonly IHiringRequestRepository _Repository;
        private readonly ILogger<HiringRequestService> _Logger;

        public HiringRequestService(ICharacterRepository CharacterRepository, IHiringRequestRepository Repository, ILogger<HiringRequestService> Logger)
        {
            _CharacterRepository = CharacterRepository;
            _Repository = Repository;
            _Logger = Logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pRequest"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(CreateHiringRequestDto pRequest)
        {
            _Logger.LogInformation("Creating hiring request for CharacterId {CharacterId}", pRequest.CharacterId);

            var lExists = await _CharacterRepository.ExistsByExternalIdAsync(pRequest.CharacterId);
            if (!lExists)
                throw new ArgumentException("Character does not exist");

            var lHiringRequest = new HiringRequest
            {
                CharacterId = pRequest.CharacterId,
                Applicant = pRequest.Applicant,
                Event = pRequest.Event,
                EventDate = pRequest.EventDate,
                Status = RequestStatus.PENDING,
                CreatedAt = DateTime.UtcNow
            };

            await _Repository.AddAsync(lHiringRequest);
            await _Repository.SaveChangesAsync();

            return lHiringRequest.Id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pQuery"></param>
        /// <returns></returns>
        public async Task<IEnumerable<HiringRequestDto>> GetFilteredAsync(HiringRequestQueryParams pQuery)
        {
            var lData = await _Repository.GetFilteredAsync(pQuery);

            return lData.Select(x => new HiringRequestDto
            {
                Id = x.Id,
                Applicant = x.Applicant,
                Event = x.Event,
                EventDate = x.EventDate,
                Status = x.Status.ToString(),
                CharacterName = x.Character.Name
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pId"></param>
        /// <returns></returns>
        public async Task<HiringRequestDto> GetByIdAsync(int pId)
        {
            var lHiringData = await _Repository.GetByIdAsync(pId) ?? throw new ArgumentException("Request not found");

            return new HiringRequestDto
            {
                Id = lHiringData.Id,
                Applicant = lHiringData.Applicant,
                Event = lHiringData.Event,
                EventDate = lHiringData.EventDate,
                Status = lHiringData.Status.ToString(),
                CharacterName = lHiringData.Character.Name
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<HiringSummaryDto> GetSummaryAsync()
        {
            var lHiringData = await _Repository.GetAllWithCharacterAsync();

            var lSummary = new HiringSummaryDto
            {
                Pending = lHiringData.Count(x => x.Status == RequestStatus.PENDING),
                InProgress = lHiringData.Count(x => x.Status == RequestStatus.IN_PROCESS),
                Approved = lHiringData.Count(x => x.Status == RequestStatus.APPROVED),
                Rejected = lHiringData.Count(x => x.Status == RequestStatus.REJECTED),

                MostRequestedCharacter = lHiringData
                    .GroupBy(x => x.Character.Name)
                    .OrderByDescending(g => g.Count())
                    .Select(g => g.Key)
                    .FirstOrDefault()
            };

            return lSummary;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pId"></param>
        /// <param name="pStatus"></param>
        /// <returns></returns>
        public async Task UpdateStatusAsync(int pId, RequestStatus pStatus)
        {
            var lHiringRow = await _Repository.GetByIdAsync(pId) ?? throw new ArgumentException("Request not found");

            lHiringRow.Status = pStatus;
            lHiringRow.UpdatedAt = DateTime.UtcNow;

            await _Repository.SaveChangesAsync();
            _Logger.LogInformation("Updated request {Id} to status {Status}", pId, pStatus);
        }
    }
}
