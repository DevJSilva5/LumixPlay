using Lumix.Data;
using Microsoft.AspNetCore.Mvc;

namespace Lumix.Controllers;

public class ProfileController : Controller
{
    private readonly AppDbContext _context;

    private readonly IWebHostEnvironment _env;

    public ProfileController(
        AppDbContext context,
        IWebHostEnvironment env
    )
    {
        _context = context;

        _env = env;
    }

    public IActionResult Index()
    {
        var email =
            HttpContext.Session.GetString("Email");

        var usuario =
            _context.Usuarios
            .FirstOrDefault(x => x.Email == email);

        return View(usuario);
    }

    [HttpPost]
    public async Task<IActionResult> Editar(
        string nome,
        IFormFile foto
    )
    {
        var email =
            HttpContext.Session.GetString("Email");

        var usuario =
            _context.Usuarios
            .FirstOrDefault(x => x.Email == email);

        if (usuario != null)
        {
            usuario.Nome = nome;

            /* FOTO */

            if (foto != null && foto.Length > 0)
            {
                var extensao =
                    Path.GetExtension(foto.FileName)
                    .ToLower();

                var nomeArquivo =
                    Guid.NewGuid().ToString()
                    + extensao;

                /* USA WWWROOT CORRETAMENTE */

                var pastaUploads =
                    Path.Combine(
                        _env.WebRootPath,
                        "uploads"
                    );

                /* CRIA PASTA */

                if (!Directory.Exists(pastaUploads))
                {
                    Directory.CreateDirectory(
                        pastaUploads
                    );
                }

                var caminhoArquivo =
                    Path.Combine(
                        pastaUploads,
                        nomeArquivo
                    );

                using (
                    var stream =
                        new FileStream(
                            caminhoArquivo,
                            FileMode.Create
                        )
                )
                {
                    await foto.CopyToAsync(stream);
                }

                /* SALVA CAMINHO */

                usuario.FotoPerfil =
                    "/uploads/" + nomeArquivo;
            }

            _context.SaveChanges();

            /* UPDATE SESSION */

            HttpContext.Session.SetString(
                "Usuario",
                usuario.Nome
            );

            HttpContext.Session.SetString(
                "FotoPerfil",
                string.IsNullOrEmpty(
                    usuario.FotoPerfil
                )
                ? "/images/perfil-default.png"
                : usuario.FotoPerfil
            );
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