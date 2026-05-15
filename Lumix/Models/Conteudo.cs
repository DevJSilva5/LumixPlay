using System.ComponentModel.DataAnnotations;

namespace Lumix.Models;

public class Conteudo
{
    public int Id { get; set; }

    [Required]
    public string Titulo { get; set; }

    public string Descricao { get; set; }

    public string Categoria { get; set; }

    public string Tipo { get; set; }

    public string Thumbnail { get; set; }

    public string Banner { get; set; }

    public string VideoUrl { get; set; }

    public int Ano { get; set; }

    public double Avaliacao { get; set; }
}