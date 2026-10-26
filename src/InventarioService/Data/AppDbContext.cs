using InventarioService.Models;
using Microsoft.EntityFrameworkCore;

namespace InventarioService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Producto> Productos => Set<Producto>();
    public DbSet<Movimiento> Movimientos => Set<Movimiento>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Nombre).IsRequired().HasMaxLength(100);
            e.Property(x => x.Descripcion).HasMaxLength(300);
            e.HasIndex(x => x.Nombre).IsUnique();
        });

        modelBuilder.Entity<Producto>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Codigo).IsRequired().HasMaxLength(50);
            e.Property(x => x.Nombre).IsRequired().HasMaxLength(150);
            e.Property(x => x.Descripcion).HasMaxLength(500);
            e.Property(x => x.Precio).HasPrecision(12, 2);
            e.HasIndex(x => x.Codigo).IsUnique();
            e.HasOne(x => x.Categoria)
                .WithMany(c => c.Productos)
                .HasForeignKey(x => x.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Movimiento>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Tipo).HasConversion<string>().HasMaxLength(20);
            e.Property(x => x.Motivo).HasMaxLength(300);
            e.HasOne(x => x.Producto)
                .WithMany(p => p.Movimientos)
                .HasForeignKey(x => x.ProductoId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
