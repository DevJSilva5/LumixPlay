using Lumix.Services;
using Microsoft.AspNetCore.Mvc;
using Lumix.Data;
using Lumix.Models;

namespace Lumix.Controllers;

public class HomeController : Controller
{
    private readonly TmdbService _tmdb;

    private readonly AppDbContext _context;

    public HomeController(
        TmdbService tmdb,
        AppDbContext context
    )
    {
        _tmdb = tmdb;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        /* PROTEÇÃO LOGIN */

        var email =
            HttpContext.Session.GetString("Email");

        if (string.IsNullOrEmpty(email))
        {
            return RedirectToAction(
                "Login",
                "Auth"
            );
        }

        /* FILMES API */

        var populares =
            await _tmdb.GetPopular();

        var acao =
            await _tmdb.GetByGenre(28);

        var romance =
            await _tmdb.GetByGenre(10749);

        var comedia =
            await _tmdb.GetByGenre(35);

        /* FAVORITOS */

        var usuario =
            _context.Usuarios
            .FirstOrDefault(x =>
                x.Email == email
            );

        List<Favorito> favoritos =
            new List<Favorito>();

        if (usuario != null)
        {
            favoritos =
                _context.Favoritos
                .Where(f =>
                    f.UsuarioId == usuario.Id
                )
                .OrderByDescending(f =>
                    f.DataFavoritado
                )
                .ToList();
        }

        /* VIEWBAG */

        ViewBag.Populares = populares;

        ViewBag.Acao = acao;

        ViewBag.Romance = romance;

        ViewBag.Comedia = comedia;

        ViewBag.Favoritos = favoritos;

        return View();
    }
}