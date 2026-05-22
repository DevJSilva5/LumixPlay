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

                var pastaUploads =
                    Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot",
                        "uploads"
                    );

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

                using (var stream =
                    new FileStream(
                        caminhoArquivo,
                        FileMode.Create,
                        FileAccess.Write,
                        FileShare.None,
                        4096,
                        true
                    ))
                {
                    await foto.CopyToAsync(stream);

                    await stream.FlushAsync();
                }

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
                usuario.FotoPerfil
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