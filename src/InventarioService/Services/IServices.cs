using InventarioService.Dtos;

namespace InventarioService.Services;

public interface ICategoriaService
{
    Task<List<CategoriaDto>> ListarAsync();
    Task<CategoriaDto> ObtenerAsync(int id);
    Task<CategoriaDto> CrearAsync(CategoriaCreateDto dto);
    Task<CategoriaDto> ActualizarAsync(int id, CategoriaUpdateDto dto);
    Task EliminarAsync(int id);
}

public interface IProductoService
{
    Task<List<ProductoDto>> ListarAsync();
    Task<ProductoDto> ObtenerAsync(int id);
    Task<ProductoDto> CrearAsync(ProductoCreateDto dto);
    Task<ProductoDto> ActualizarAsync(int id, ProductoUpdateDto dto);
    Task EliminarAsync(int id);
}

public interface IMovimientoService
{
    Task<List<MovimientoDto>> ListarAsync();
    Task<List<MovimientoDto>> ListarPorProductoAsync(int productoId);
    Task<MovimientoDto> ObtenerAsync(int id);
    Task<MovimientoDto> RegistrarAsync(MovimientoCreateDto dto);
}
