using PruebaTecnicaKibernum.Application.Dtos.ApiDto;
using PruebaTecnicaKibernum.Application.Interfaces;
using System.Net;

namespace PruebaTecnicaKibernum.Infrastructure.ExternalServices
{
    public class RickAndMortyService : IRickAndMortyService
    {
        private readonly HttpClient _HttpClient;

        public RickAndMortyService(HttpClient HttpClient)
        {
            _HttpClient = HttpClient;
        }

        /// <summary>
        /// Obtiene una página de personajes desde la API externa de Rick & Morty
        /// </summary>
        /// <param name="pPage">Número de página a consultar</param>
        /// <returns>
        /// Un objeto con la información de los personajes,
        /// o null si no se puede deserializar la respuesta.
        /// </returns>
        /// <exception cref="Exception">
        /// Se lanza cuando se excede el número de reintentos debido a limitaciones de la API
        /// </exception>
        /// <remarks>
        /// Este método consume la API externa utilizando HttpClient e implementa
        /// un mecanismo de resiliencia frente a errores de tipo "Too Many Requests" (HTTP 429).
        /// </remarks>
        public async Task<RickAndMortyResponseDto?> GetCharactersAsync(int pPage)
        {
            int lRetries = 3;

            for (int i = 0; i < lRetries; i++)
            {
                var lApiResponse = await _HttpClient.GetAsync($"character?page={pPage}");

                if (lApiResponse.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    await Task.Delay(1000 * (i + 1));
                    continue;
                }

                lApiResponse.EnsureSuccessStatusCode();
                return await lApiResponse.Content.ReadFromJsonAsync<RickAndMortyResponseDto>();
            }

            throw new Exception("Rate limit exceeded after retries");
        }
    }
}
