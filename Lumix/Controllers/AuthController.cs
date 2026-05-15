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

    public IActionResult Login()
    {
        return View();
    }

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

        usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha);

        _context.Usuarios.Add(usuario);

        _context.SaveChanges();

        return RedirectToAction("Login");
    }

    [HttpPost]
    public IActionResult Login(string email, string senha)
    {
        var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == email);

        if (usuario == null)
        {
            ViewBag.Erro = "Usuário não encontrado";
            return View();
        }

        bool senhaCorreta = BCrypt.Net.BCrypt.Verify(senha, usuario.Senha);

        if (!senhaCorreta)
        {
            ViewBag.Erro = "Senha incorreta";
            return View();
        }

        HttpContext.Session.SetString("Usuario", usuario.Nome);

        return RedirectToAction("Index", "Home");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();

        return RedirectToAction("Login");
    }
}