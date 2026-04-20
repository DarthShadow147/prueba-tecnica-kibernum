using PruebaTecnicaKibernum.Application.Dtos.HiringDto;
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
        /// <returns></returns>
        public async Task<IEnumerable<HiringRequestDto>> GetAllAsync()
        {
            var lData = await _Repository.GetAllAsync();

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
