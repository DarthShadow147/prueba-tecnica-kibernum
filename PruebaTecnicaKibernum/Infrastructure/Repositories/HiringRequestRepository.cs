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
        /// Agrega una nueva solicitud de contratación al contexto de la base de datos
        /// </summary>
        /// <param name="pRequest">Entidad de la solicitud a agregar</param>
        /// <returns>
        /// Tarea asincrónica que representa la operación
        /// </returns>
        /// <remarks>
        /// Este método agrega la solicitud al contexto de Entity Framework,
        /// pero no persiste los cambios inmediatamente en la base de datos
        /// </remarks>
        public async Task AddAsync(HiringRequest pRequest)
        {
            await _Context.HiringRequest.AddAsync(pRequest);
        }

        /// <summary>
        /// Obtiene un listado de solicitudes de contratación aplicando filtros opcionales
        /// </summary>
        /// <param name="pQuery">
        /// Parámetros de consulta que permiten filtrar por estado y solicitante
        /// </param>
        /// <returns>
        /// Lista de entidades <see cref="HiringRequest"/> que cumplen los criterios de filtrado
        /// </returns>
        /// <remarks>
        /// Este método construye dinámicamente una consulta utilizando <c>IQueryable</c>
        /// para aplicar filtros antes de ejecutar la consulta en base de datos
        /// </remarks>
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
        /// Obtiene una solicitud de contratación por su identificador, incluyendo el personaje asociado
        /// </summary>
        /// <param name="pId">Identificador de la solicitud</param>
        /// <returns>
        /// La entidad <see cref="HiringRequest"/> si existe; de lo contrario, <c>null</c>
        /// </returns>
        /// <remarks>
        /// Este método consulta la base de datos para obtener una solicitud específica,
        /// incluyendo la entidad relacionada <c>Character</c> mediante <c>Include</c>
        /// </remarks>
        public async Task<HiringRequest?> GetByIdAsync(int pId)
        {
            return await _Context.HiringRequest
                .Include(x => x.Character)
                .FirstOrDefaultAsync(x => x.Id == pId);
        }

        /// <summary>
        /// Obtiene todas las solicitudes de contratación incluyendo la información del personaje asociado
        /// </summary>
        /// <returns>
        /// Lista de entidades <see cref="HiringRequest"/> con sus relaciones cargadas
        /// </returns>
        /// <remarks>
        /// Este método recupera todas las solicitudes desde la base de datos,
        /// incluyendo la entidad relacionada <c>Character</c> mediante <c>Include</c>.
        /// </remarks>
        public async Task<List<HiringRequest>> GetAllWithCharacterAsync()
        {
            return await _Context.HiringRequest
                .Include(x => x.Character)
                .ToListAsync();
        }

        /// <summary>
        /// Persiste los cambios realizados en el contexto de la base de datos
        /// </summary>
        /// <returns>
        /// Tarea asincrónica que representa la operación de guardado
        /// </returns>
        /// <remarks>
        /// Este método ejecuta <c>SaveChangesAsync</c> sobre el contexto de Entity Framework,
        /// aplicando todas las operaciones pendientes (insert, update, delete)
        /// </remarks>
        public async Task SaveChangesAsync()
        {
            await _Context.SaveChangesAsync();
        }
    }
}
