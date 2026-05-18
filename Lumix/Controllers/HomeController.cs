using Lumix.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lumix.Controllers;

public class HomeController : Controller
{
    private readonly TmdbService _tmdb;

    public HomeController(TmdbService tmdb)
    {
        _tmdb = tmdb;
    }

    public async Task<IActionResult> Index()
    {
        var populares = await _tmdb.GetPopular();

        var acao = await _tmdb.GetByGenre(28);

        var romance = await _tmdb.GetByGenre(10749);

        var comedia = await _tmdb.GetByGenre(35);

        ViewBag.Populares = populares;
        ViewBag.Acao = acao;
        ViewBag.Romance = romance;
        ViewBag.Comedia = comedia;

        return View();
    }
}