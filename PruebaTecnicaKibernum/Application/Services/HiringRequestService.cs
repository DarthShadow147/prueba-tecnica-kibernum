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
        /// Crea una nueva solicitud de contratación de un personaje
        /// </summary>
        /// <param name="pRequest">
        /// Objeto que contiene la información de la solicitud:
        /// personaje, solicitante, evento y fecha del evento
        /// </param>
        /// <returns>
        /// Identificador de la solicitud creada
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Se lanza cuando el personaje asociado no existe en la base de datos
        /// </exception>
        /// <remarks>
        /// Este método realiza las siguientes acciones:
        /// 
        /// - Registra en logs la creación de la solicitud
        /// - Valida que el personaje exista antes de crear la solicitud
        /// - Inicializa el estado de la solicitud como PENDING
        /// - Asigna la fecha de creación en UTC
        /// - Persiste la solicitud en la base de datos
        /// </remarks>
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
        /// Obtiene un listado de solicitudes de contratación aplicando filtros opcionales
        /// </summary>
        /// <param name="pQuery">Parámetros de consulta que permiten filtrar por estado y solicitante</param>
        /// <returns>
        /// Una colección con la información de las solicitudes
        /// </returns>
        /// <remarks>
        /// Este método delega la obtención de datos al repositorio, el cual aplica
        /// filtros dinámicos sobre la base de datos
        /// 
        /// Posteriormente, transforma las entidades a DTOs para evitar exponer
        /// directamente el modelo de dominio
        /// </remarks>
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
        /// Obtiene el detalle de una solicitud de contratación por su identificador
        /// </summary>
        /// <param name="pId">Identificador de la solicitud</param>
        /// <returns>
        /// Un objeto con la información de la solicitud
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Se lanza cuando no se encuentra una solicitud con el identificador proporcionado
        /// </exception>
        /// <remarks>
        /// Este método consulta el repositorio para obtener la solicitud por su ID
        /// 
        /// Si la solicitud no existe, se lanza una excepción controlada que será
        /// manejada por el middleware global, retornando una respuesta consistente
        /// </remarks>
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
        /// Obtiene un resumen de las solicitudes de contratación
        /// </summary>
        /// <returns>
        /// Un objeto que contiene:
        /// - Totales de solicitudes por estado
        /// - El personaje más solicitado
        /// </returns>
        /// <remarks>
        /// Este método obtiene todas las solicitudes junto con su personaje asociado
        /// y calcula métricas agregadas mediante operaciones LINQ
        /// </remarks>
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
        /// Actualiza el estado de una solicitud de contratación
        /// </summary>
        /// <param name="pId">Identificador de la solicitud</param>
        /// <param name="pStatus">Nuevo estado de la solicitud</param>
        /// <returns>
        /// Tarea asincrónica que representa la operación
        /// </returns>
        /// <remarks>
        /// Este método realiza las siguientes acciones:
        /// 
        /// - Obtiene la solicitud desde la base de datos
        /// - Valida su existencia
        /// - Actualiza el estado de la solicitud
        /// - Registra la fecha de última modificación en UTC
        /// - Persiste los cambios
        /// - Registra la operación en logs
        /// </remarks>
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
