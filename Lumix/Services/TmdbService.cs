using System.Text.Json;

namespace Lumix.Services;

public class TmdbService
{
    private readonly HttpClient _httpClient;

    private const string API_KEY =
        "5f65a615f82aec9b40abfb17c173e794";

    public TmdbService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<JsonDocument> GetPopular()
    {
        var url =
            $"https://api.themoviedb.org/3/movie/popular?api_key={API_KEY}&language=pt-BR&include_adult=false";

        return await GetData(url);
    }

    public async Task<JsonDocument> GetByGenre(int genreId)
    {
        var url =
            $"https://api.themoviedb.org/3/discover/movie?api_key={API_KEY}&with_genres={genreId}&language=pt-BR&include_adult=false";

        return await GetData(url);
    }

    /* TRATAMENTO DE ERRO */

    private async Task<JsonDocument> GetData(string url)
    {
        try
        {
            var response =
                await _httpClient.GetStringAsync(url);

            return JsonDocument.Parse(response);
        }
        catch
        {
            /* EVITA CRASH */

            return JsonDocument.Parse(
                """
                {
                    "results": []
                }
                """
            );
        }
    }
}