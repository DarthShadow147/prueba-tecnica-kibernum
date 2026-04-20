using PruebaTecnicaKibernum.Application.Dtos.CharacterDto;
using PruebaTecnicaKibernum.Application.Interfaces;
using PruebaTecnicaKibernum.Domain.Entities;

namespace PruebaTecnicaKibernum.Application.Services
{
    public class CharacterService : ICharacterService
    {
        private readonly ICharacterRepository _Repository;
        private readonly IRickAndMortyService _ApiService;
        private readonly ILogger<CharacterService> _Logger;

        public CharacterService(
            ICharacterRepository Repository, 
            IRickAndMortyService ApiService, 
            ILogger<CharacterService> Logger)
        {
            _Repository = Repository;
            _ApiService = ApiService;
            _Logger = Logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task ImportCharacterAsync()
        {
            int lPage = 1;

            while (true)
            {
                _Logger.LogInformation("Fetching page {Page}", lPage);

                var lApiResponse = await _ApiService.GetCharactersAsync(lPage);

                if (lApiResponse == null || lApiResponse.Results.Count == 0)
                    break;

                foreach (var lCharacter in lApiResponse.Results)
                {
                    if (await _Repository.ExistsByExternalIdAsync(lCharacter.Id))
                        continue;

                    await _Repository.AddAsync(new Character
                    {
                        ExternalId = lCharacter.Id,
                        Name = lCharacter.Name,
                        Status = lCharacter.Status,
                        Species = lCharacter.Species,
                        Gender = lCharacter.Gender,
                        Origin = lCharacter.Origin.Name,
                        ImageUrl = lCharacter.Image,
                        ImportedAt = DateTime.UtcNow
                    });
                }

                await _Repository.SaveChangesAsync();
                lPage++;
                await Task.Delay(500);

                if (lPage > lApiResponse.Info.Pages)
                    break;
            }

            _Logger.LogInformation("Character import completed");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pQuery"></param>
        /// <returns></returns>
        public async Task<PagedResult<CharacterResumeDto>> GetPagedAsync(CharacterQueryParameters pQuery)
        {
            var (lData, lTotal) = await _Repository.GetPagedAsync(pQuery);

            var lResult = lData.Select(x => new CharacterResumeDto
            {
                Id = x.Id,
                Name = x.Name,
                Status = x.Status,
                Species = x.Species,
                Gender = x.Gender,
                Origin = x.Origin,
                ImageUrl = x.ImageUrl
            });

            return new PagedResult<CharacterResumeDto>
            {
                Items = lResult,
                TotalCount = lTotal,
                Page = pQuery.Page,
                PageSize = pQuery.PageSize
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pId"></param>
        /// <returns></returns>
        public async Task<CharacterResumeDto?> GetCharacterByIdAsync(int pId)
        {
            var lCharacterDetail = await _Repository.GetByIdAsync(pId) ?? throw new ArgumentException("Character not found");

            return new CharacterResumeDto
            {
                Id = lCharacterDetail.Id,
                Name = lCharacterDetail.Name,
                Status = lCharacterDetail.Status,
                Species = lCharacterDetail.Species,
                Gender = lCharacterDetail.Gender,
                Origin = lCharacterDetail.Origin,
                ImageUrl = lCharacterDetail.ImageUrl
            };
        }
    }
}
