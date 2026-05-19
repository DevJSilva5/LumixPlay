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
            $"https://api.themoviedb.org/3/discover/movie" +
            $"?api_key={API_KEY}" +
            $"&language=pt-BR" +
            $"&sort_by=popularity.desc" +
            $"&include_adult=false" +
            $"&include_video=false" +
            $"&vote_average.gte=6" +
            $"&vote_count.gte=200" +
            $"&without_genres=99" +
            $"&certification_country=BR" +
            $"&certification.lte=14";

        return await GetData(url);
    }

    public async Task<JsonDocument> GetByGenre(int genreId)
    {
        var url =
            $"https://api.themoviedb.org/3/discover/movie" +
            $"?api_key={API_KEY}" +
            $"&with_genres={genreId}" +
            $"&language=pt-BR" +
            $"&sort_by=popularity.desc" +
            $"&include_adult=false" +
            $"&include_video=false" +
            $"&vote_average.gte=6" +
            $"&vote_count.gte=150" +
            $"&without_genres=99" +
            $"&certification_country=BR" +
            $"&certification.lte=14";

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

    public async Task<JsonDocument> GetMovieDetails(int movieId)
    {
        var url =
            $"https://api.themoviedb.org/3/movie/{movieId}" +
            $"?api_key={API_KEY}" +
            $"&language=pt-BR";

        return await GetData(url);
    }

    public async Task<JsonDocument> GetMovieVideos(int movieId)
    {
        var url =
            $"https://api.themoviedb.org/3/movie/{movieId}/videos" +
            $"?api_key={API_KEY}" +
            $"&language=pt-BR";

        return await GetData(url);
    }
}

