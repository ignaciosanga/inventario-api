using InventarioService.Models;

namespace InventarioService.Repositories;

public interface IRepository<T> where T : class
{
    Task<List<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}

public interface ICategoriaRepository : IRepository<Categoria>
{
    Task<bool> ExisteNombreAsync(string nombre, int? excluirId = null);
    Task<bool> TieneProductosAsync(int categoriaId);
}

public interface IProductoRepository : IRepository<Producto>
{
    Task<Producto?> GetByCodigoAsync(string codigo);
    Task<bool> ExisteCodigoAsync(string codigo, int? excluirId = null);
}

public interface IMovimientoRepository : IRepository<Movimiento>
{
    Task<List<Movimiento>> GetByProductoAsync(int productoId);
}
