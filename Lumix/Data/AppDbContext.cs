using Lumix.Models;
using Microsoft.EntityFrameworkCore;

namespace Lumix.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; }

    public DbSet<Favorito> Favoritos { get; set; }

    public DbSet<Conteudo> Conteudos { get; set; }
}