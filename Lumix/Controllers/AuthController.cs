using BCrypt.Net;
using Lumix.Data;
using Lumix.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

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
        var usuario = HttpContext.Session.GetString("Usuario");
        if (!string.IsNullOrEmpty(usuario))
        {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }

    /* REGISTER */
    public IActionResult Register()
    {
        var usuario = HttpContext.Session.GetString("Usuario");
        if (!string.IsNullOrEmpty(usuario))
        {
            return RedirectToAction("Index", "Home");
        }
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

        if (string.IsNullOrEmpty(usuario.FotoPerfil))
        {
            usuario.FotoPerfil = "/images/perfil-default.png";
        }

        _context.Usuarios.Add(usuario);
        _context.SaveChanges();

        HttpContext.Session.SetString("Usuario", usuario.Nome);
        HttpContext.Session.SetString("Email", usuario.Email);
        HttpContext.Session.SetString("FotoPerfil", usuario.FotoPerfil);

        HttpContext.Response.Cookies.Append(
            "LumixLogin",
            "true",
            new CookieOptions
            {
                Expires = DateTime.Now.AddDays(30),
                HttpOnly = true,
                IsEssential = true
            }
        );

        return RedirectToAction("Index", "Home");
    }

    /* LOGIN POST */
    [HttpPost]
    public IActionResult Login(string email, string senha, bool continuarLogado)
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
        HttpContext.Session.SetString("Email", usuario.Email);
        HttpContext.Session.SetString("FotoPerfil", string.IsNullOrEmpty(usuario.FotoPerfil) ? "/images/perfil-default.png" : usuario.FotoPerfil);

        if (continuarLogado)
        {
            HttpContext.Session.SetString("ContinuarLogado", "true");
            HttpContext.Response.Cookies.Append(
                "LumixLogin",
                "true",
                new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(30),
                    HttpOnly = true,
                    IsEssential = true
                }
            );
        }
        else
        {
            HttpContext.Response.Cookies.Delete("LumixLogin");
        }

        return RedirectToAction("Index", "Home");
    }

    /* LOGOUT */
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        HttpContext.Response.Cookies.Delete("LumixLogin");
        return RedirectToAction("Login", "Auth");
    }

    /* RESETAR SENHA */
    [HttpPost]
    public IActionResult ResetPassword(string email, string novaSenha, string confirmarSenha)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(novaSenha) || string.IsNullOrWhiteSpace(confirmarSenha))
        {
            return BadRequest("Preencha todos os campos");
        }

        if (novaSenha != confirmarSenha)
        {
            return BadRequest("As senhas não coincidem");
        }

        var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == email);

        if (usuario == null)
        {
            return NotFound("Email não encontrado");
        }

        usuario.Senha = BCrypt.Net.BCrypt.HashPassword(novaSenha);
        _context.Usuarios.Update(usuario);
        _context.SaveChanges();

        return Ok("Senha alterada com sucesso!");
    }
}