namespace Lumix.Models;

public class Favorito
{
    public int Id { get; set; }

    public int FilmeId { get; set; }

    public string Titulo { get; set; } = string.Empty;

    public string Poster { get; set; } = string.Empty;

    public DateTime DataFavoritado { get; set; }

    public int UsuarioId { get; set; }

    public Usuario? Usuario { get; set; }
}