using InventarioService.Data;
using InventarioService.Models;
using Microsoft.EntityFrameworkCore;

namespace InventarioService.Repositories;

public class MovimientoRepository : Repository<Movimiento>, IMovimientoRepository
{
    public MovimientoRepository(AppDbContext context) : base(context)
    {
    }

    public override async Task<List<Movimiento>> GetAllAsync()
    {
        return await Set.AsNoTracking()
            .Include(m => m.Producto)
            .OrderByDescending(m => m.Fecha)
            .ToListAsync();
    }

    public override async Task<Movimiento?> GetByIdAsync(int id)
    {
        return await Set.Include(m => m.Producto).FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<List<Movimiento>> GetByProductoAsync(int productoId)
    {
        return await Set.AsNoTracking()
            .Include(m => m.Producto)
            .Where(m => m.ProductoId == productoId)
            .OrderByDescending(m => m.Fecha)
            .ToListAsync();
    }
}
