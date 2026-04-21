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
        /// Importa personajes desde la API pública de Rick & Morty y los persiste en la base de datos local
        /// </summary>
        /// <remarks>
        /// Este proceso realiza la importación de forma paginada, consultando la API externa página por página
        /// 
        /// Para cada personaje:
        /// - Verifica si ya existe en la base de datos utilizando el ExternalId
        /// - Evita duplicados
        /// - Mapea los datos al modelo interno
        /// 
        /// El proceso finaliza cuando:
        /// - No hay más resultados en la API
        /// - Se alcanza el número total de páginas disponibles
        /// </remarks>
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
        /// Obtiene un listado paginado de personajes aplicando filtros opcionales
        /// </summary>
        /// <param name="pQuery">Parámetros de consulta que incluyen paginación (Page, PageSize) y filtros opcionales
        /// como nombre y estado del personaje
        /// </param>
        /// <returns>Un objeto que contiene
        /// - Lista de personajes (CharacterResumeDto)
        /// - Total de registros disponibles
        /// - Información de paginación (página actual y tamaño de página)
        /// </returns>
        /// <remarks>
        /// Este método delega la obtención de datos al repositorio, el cual ejecuta la consulta
        /// contra la base de datos aplicando filtros dinámicos y paginación
        /// </remarks>
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
        /// Obtiene el detalle de un personaje específico por su identificador
        /// </summary>
        /// <param name="pId">Identificador del personaje en la base de datos</param>
        /// <returns>
        /// Un objeto con la información del personaje
        /// o una excepción si no existe.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Se lanza cuando no se encuentra un personaje con el identificador proporcionado
        /// </exception>
        /// <remarks>
        /// Este método consulta el repositorio para obtener el personaje por su ID
        /// 
        /// Si el personaje no existe, se lanza una excepción controlada que será
        /// capturada por el middleware global de manejo de errores
        /// </remarks>
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
