using PruebaTecnicaKibernum.Application.Dtos.ApiDto;

namespace PruebaTecnicaKibernum.Application.Interfaces
{
    public interface IRickAndMortyService
    {
        Task<RickAndMortyResponseDto?> GetCharactersAsync(int pPage);
    }
}
