using InventarioService.Data;
using InventarioService.Models;
using Microsoft.EntityFrameworkCore;

namespace InventarioService.Repositories;

public class ProductoRepository : Repository<Producto>, IProductoRepository
{
    public ProductoRepository(AppDbContext context) : base(context)
    {
    }

    public override async Task<List<Producto>> GetAllAsync()
    {
        return await Set.AsNoTracking()
            .Include(p => p.Categoria)
            .OrderBy(p => p.Id)
            .ToListAsync();
    }

    public override async Task<Producto?> GetByIdAsync(int id)
    {
        return await Set.Include(p => p.Categoria).FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Producto?> GetByCodigoAsync(string codigo)
    {
        return await Set.Include(p => p.Categoria)
            .FirstOrDefaultAsync(p => p.Codigo == codigo);
    }

    public async Task<bool> ExisteCodigoAsync(string codigo, int? excluirId = null)
    {
        return await Set.AnyAsync(p =>
            p.Codigo == codigo && (excluirId == null || p.Id != excluirId));
    }
}
