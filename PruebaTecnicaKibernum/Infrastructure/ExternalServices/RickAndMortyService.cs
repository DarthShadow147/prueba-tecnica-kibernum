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
        /// 
        /// </summary>
        /// <param name="pPage"></param>
        /// <returns></returns>
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
