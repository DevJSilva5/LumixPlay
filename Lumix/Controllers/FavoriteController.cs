using Lumix.Data;
using Lumix.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lumix.Controllers;

public class FavoriteController : Controller
{
    private readonly AppDbContext _context;

    public FavoriteController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult Toggle(
        int filmeId,
        string titulo,
        string poster
    )
    {
        var email =
            HttpContext.Session.GetString("Email");

        if (string.IsNullOrEmpty(email))
        {
            return RedirectToAction(
                "Login",
                "Auth"
            );
        }

        var usuario =
            _context.Usuarios
            .FirstOrDefault(x => x.Email == email);

        if (usuario == null)
        {
            return RedirectToAction(
                "Login",
                "Auth"
            );
        }

        var favorito =
            _context.Favoritos.FirstOrDefault(f =>
                f.UsuarioId == usuario.Id
                &&
                f.FilmeId == filmeId
            );

        /* REMOVE FAVORITO */

        if (favorito != null)
        {
            _context.Favoritos.Remove(favorito);

            _context.SaveChanges();

            return Redirect(
                $"/Movie/Details/{filmeId}"
            );
        }

        /* ADICIONA FAVORITO */

        var novoFavorito = new Favorito
        {
            FilmeId = filmeId,
            Titulo = titulo,
            Poster = poster,
            UsuarioId = usuario.Id,
            DataFavoritado = DateTime.Now
        };

        _context.Favoritos.Add(novoFavorito);

        _context.SaveChanges();

        return Redirect(
            $"/Movie/Details/{filmeId}"
        );
    }
}