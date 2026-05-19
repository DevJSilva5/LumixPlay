using Lumix.Data;
using Lumix.Models;
using Lumix.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lumix.Controllers;

public class MovieController : Controller
{
    private readonly TmdbService _tmdb;

    private readonly AppDbContext _context;

    public MovieController(
        TmdbService tmdb,
        AppDbContext context
    )
    {
        _tmdb = tmdb;
        _context = context;
    }

    public async Task<IActionResult> Details(int id)
    {
        var filme =
            await _tmdb.GetMovieDetails(id);

        var videos =
            await _tmdb.GetMovieVideos(id);

        ViewBag.Movie = filme;

        ViewBag.Videos = videos;

        /* FAVORITADO */

        var email =
            HttpContext.Session.GetString("Email");

        bool favoritado = false;

        if (!string.IsNullOrEmpty(email))
        {
            var usuario =
                _context.Usuarios
                .FirstOrDefault(u => u.Email == email);

            if (usuario != null)
            {
                favoritado =
                    _context.Favoritos.Any(f =>
                        f.UsuarioId == usuario.Id
                        &&
                        f.FilmeId == id
                    );
            }
        }

        ViewBag.Favoritado = favoritado;

        return View();
    }

    [HttpPost]
    public IActionResult ToggleFavoritoAjax(
        [FromBody] FavoritoRequest request
    )
    {
        var email =
            HttpContext.Session.GetString("Email");

        if (string.IsNullOrEmpty(email))
        {
            return Json(new
            {
                success = false
            });
        }

        var usuario =
            _context.Usuarios
            .FirstOrDefault(u => u.Email == email);

        if (usuario == null)
        {
            return Json(new
            {
                success = false
            });
        }

        var favoritoExistente =
            _context.Favoritos
            .FirstOrDefault(f =>
                f.UsuarioId == usuario.Id
                &&
                f.FilmeId == request.FilmeId
            );

        /* REMOVE */

        if (favoritoExistente != null)
        {
            _context.Favoritos.Remove(
                favoritoExistente
            );

            _context.SaveChanges();

            return Json(new
            {
                favoritado = false
            });
        }

        /* ADICIONA */

        var favorito = new Favorito
        {
            FilmeId = request.FilmeId,

            Titulo = request.Titulo,

            Poster = request.Poster,

            UsuarioId = usuario.Id,

            DataFavoritado = DateTime.Now
        };

        _context.Favoritos.Add(favorito);

        _context.SaveChanges();

        return Json(new
        {
            favoritado = true
        });
    }
}