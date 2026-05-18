using Lumix.Data;
using Microsoft.AspNetCore.Mvc;

namespace Lumix.Controllers;

public class ProfileController : Controller
{
    private readonly AppDbContext _context;

    public ProfileController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var email = HttpContext.Session.GetString("Email");

        var usuario = _context.Usuarios
            .FirstOrDefault(x => x.Email == email);

        return View(usuario);
    }

    [HttpPost]
    public IActionResult Editar(string nome, string fotoPerfil)
    {
        var email = HttpContext.Session.GetString("Email");

        var usuario = _context.Usuarios
            .FirstOrDefault(x => x.Email == email);

        if (usuario != null)
        {
            usuario.Nome = nome;

            if (!string.IsNullOrEmpty(fotoPerfil))
            {
                usuario.FotoPerfil = fotoPerfil;
            }

            _context.SaveChanges();

            HttpContext.Session.SetString("Usuario", usuario.Nome);
        }

        return RedirectToAction("Index");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();

        return RedirectToAction(
            "Login",
            "Auth"
        );
    }
}