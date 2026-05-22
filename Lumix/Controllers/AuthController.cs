using BCrypt.Net;
using Lumix.Data;
using Lumix.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lumix.Controllers;

public class AuthController : Controller
{
    private readonly AppDbContext _context;

    public AuthController(AppDbContext context)
    {
        _context = context;
    }

    /* LOGIN */

    public IActionResult Login()
    {
        var usuario =
            HttpContext.Session.GetString("Usuario");

        /* SE ESTIVER LOGADO */

        if (!string.IsNullOrEmpty(usuario))
        {
            return RedirectToAction(
                "Index",
                "Home"
            );
        }

        return View();
    }

    /* REGISTER */

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(Usuario usuario)
    {
        if (_context.Usuarios.Any(u => u.Email == usuario.Email))
        {
            ViewBag.Erro = "Email já cadastrado";

            return View();
        }

        usuario.Senha =
            BCrypt.Net.BCrypt.HashPassword(
                usuario.Senha
            );

        _context.Usuarios.Add(usuario);

        _context.SaveChanges();

        return RedirectToAction("Login");
    }

    /* LOGIN POST */

    [HttpPost]
    public IActionResult Login(
        string email,
        string senha,
        bool continuarLogado
    )
    {
        var usuario = _context.Usuarios
            .FirstOrDefault(u => u.Email == email);

        if (usuario == null)
        {
            ViewBag.Erro = "Usuário não encontrado";

            return View();
        }

        bool senhaCorreta =
            BCrypt.Net.BCrypt.Verify(
                senha,
                usuario.Senha
            );

        if (!senhaCorreta)
        {
            ViewBag.Erro = "Senha incorreta";

            return View();
        }

        /* SESSION */

        HttpContext.Session.SetString(
            "Usuario",
            usuario.Nome
        );

        HttpContext.Session.SetString(
            "Email",
            usuario.Email
        );

        /* FOTO */

        if (!string.IsNullOrEmpty(usuario.FotoPerfil))
        {
            HttpContext.Session.SetString(
                "FotoPerfil",
                usuario.FotoPerfil
            );
        }
        else
        {
            HttpContext.Session.SetString(
                "FotoPerfil",
                "/images/perfil-default.png"
            );
        }

        /* CONTINUAR LOGADO */

        if (continuarLogado)
        {
            HttpContext.Session.SetString(
                "ContinuarLogado",
                "true"
            );

            HttpContext.Response.Cookies.Append(
                "LumixLogin",
                "true",
                new CookieOptions
                {
                    Expires =
                        DateTime.Now.AddDays(30),

                    HttpOnly = true,

                    IsEssential = true
                }
            );
        }
        else
        {
            HttpContext.Response.Cookies.Delete(
                "LumixLogin"
            );
        }

        return RedirectToAction(
            "Index",
            "Home"
        );
    }

    /* LOGOUT */

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();

        HttpContext.Response.Cookies.Delete(
            "LumixLogin"
        );

        return RedirectToAction(
            "Login",
            "Auth"
        );
    }
}