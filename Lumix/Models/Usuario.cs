using System.ComponentModel.DataAnnotations;

namespace Lumix.Models;

public class Usuario
{
    public int Id { get; set; }

    [Required]
    public string Nome { get; set; } = string.Empty;

    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Senha { get; set; } = string.Empty;

    public string FotoPerfil { get; set; } =
        "/images/perfil-default.png";

}