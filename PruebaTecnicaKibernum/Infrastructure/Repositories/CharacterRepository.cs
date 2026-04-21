using Microsoft.EntityFrameworkCore;
using PruebaTecnicaKibernum.Application.Dtos.CharacterDto;
using PruebaTecnicaKibernum.Application.Interfaces;
using PruebaTecnicaKibernum.Domain.Entities;
using PruebaTecnicaKibernum.Infrastructure.DataContext;

namespace PruebaTecnicaKibernum.Infrastructure.Repositories
{
    public class CharacterRepository : ICharacterRepository
    {
        private readonly AppDbContext _Context;

        public CharacterRepository(AppDbContext Context)
        {
            _Context = Context;
        }

        /// <summary>
        /// Verifica si existe un personaje en la base de datos según su identificador externo
        /// </summary>
        /// <param name="pExternalId">Identificador del personaje en la API externa</param>
        /// <returns>
        /// <c>true</c> si el personaje ya existe en la base de datos; de lo contrario, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// Este método se utiliza principalmente durante el proceso de importación de personajes
        /// para evitar duplicados
        /// </remarks>
        public async Task<bool> ExistsByExternalIdAsync(int pExternalId)
        {
            return await _Context.Character
                .AnyAsync(c => c.ExternalId == pExternalId);
        }

        /// <summary>
        /// Agrega un nuevo personaje al contexto de la base de datos
        /// </summary>
        /// <param name="pCharacter">Entidad del personaje a agregar</param>
        /// <returns>
        /// Tarea asincrónica que representa la operación
        /// </returns>
        /// <remarks>
        /// Este método agrega la entidad al contexto, pero no persiste los cambios
        /// inmediatamente en la base de datos
        /// </remarks>
        public async Task AddAsync(Character pCharacter)
        {
            await _Context.Character.AddAsync(pCharacter);
        }

        /// <summary>
        /// Obtiene un listado paginado de personajes aplicando filtros opcionales
        /// </summary>
        /// <param name="pQuery">
        /// Parámetros de consulta que incluyen:
        /// - Filtros opcionales (nombre, estado)
        /// - Paginación (Page, PageSize)
        /// </param>
        /// <returns>
        /// Una tupla que contiene:
        /// - Lista de personajes filtrados y paginados
        /// - Total de registros que cumplen los filtros (sin paginación)
        /// </returns>
        /// <remarks>
        /// Este método construye dinámicamente una consulta utilizando <c>IQueryable</c>
        /// para permitir la aplicación de filtros antes de ejecutar la consulta en base de datos.
        /// </remarks>
        public async Task<(List<Character> Data, int TotalCount)> GetPagedAsync(CharacterQueryParameters pQuery)
        {
            var lDbQuery = _Context.Character.AsQueryable();

            if (!string.IsNullOrWhiteSpace(pQuery.Name))
                lDbQuery = lDbQuery.Where(x => x.Name.Contains(pQuery.Name));

            if (!string.IsNullOrWhiteSpace(pQuery.Status))
                lDbQuery = lDbQuery.Where(x => x.Status == pQuery.Status);

            var lTotalCount = await lDbQuery.CountAsync();

            var lData = await lDbQuery
                .Skip((pQuery.Page - 1) * pQuery.PageSize)
                .Take(pQuery.PageSize)
                .ToListAsync();

            return (lData, lTotalCount);
        }

        /// <summary>
        /// Obtiene un personaje por su identificador
        /// </summary>
        /// <param name="pId">Identificador del personaje</param>
        /// <returns>
        /// La entidad <see cref="Character"/> si existe; de lo contrario, <c>null</c>
        /// </returns>
        /// <remarks>
        /// Este método realiza una consulta a la base de datos para obtener un personaje
        /// específico según su ID
        /// </remarks>
        public async Task<Character?> GetByIdAsync(int pId)
        {
            return await _Context.Character
                .FirstOrDefaultAsync(x => x.Id == pId);
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
