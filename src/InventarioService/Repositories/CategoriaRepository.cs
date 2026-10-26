using InventarioService.Data;
using InventarioService.Models;
using Microsoft.EntityFrameworkCore;

namespace InventarioService.Repositories;

public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
{
    public CategoriaRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<bool> ExisteNombreAsync(string nombre, int? excluirId = null)
    {
        var normalizado = nombre.Trim().ToLower();
        return await Set.AnyAsync(c =>
            c.Nombre.ToLower() == normalizado && (excluirId == null || c.Id != excluirId));
    }

    public async Task<bool> TieneProductosAsync(int categoriaId)
    {
        return await Context.Productos.AnyAsync(p => p.CategoriaId == categoriaId);
    }
}
