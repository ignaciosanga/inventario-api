using InventarioService.Dtos;
using InventarioService.Exceptions;
using InventarioService.Mappings;
using InventarioService.Models;
using InventarioService.Repositories;

namespace InventarioService.Services;

public class ProductoService : IProductoService
{
    private readonly IProductoRepository _productos;
    private readonly ICategoriaRepository _categorias;

    public ProductoService(IProductoRepository productos, ICategoriaRepository categorias)
    {
        _productos = productos;
        _categorias = categorias;
    }

    public async Task<List<ProductoDto>> ListarAsync()
    {
        var productos = await _productos.GetAllAsync();
        return productos.Select(p => p.ToDto()).ToList();
    }

    public async Task<ProductoDto> ObtenerAsync(int id)
    {
        var producto = await _productos.GetByIdAsync(id)
            ?? throw new NotFoundException($"No existe el producto con id {id}.");
        return producto.ToDto();
    }

    public async Task<ProductoDto> CrearAsync(ProductoCreateDto dto)
    {
        var codigo = dto.Codigo.Trim();

        if (await _categorias.GetByIdAsync(dto.CategoriaId) is null)
        {
            throw new NotFoundException($"No existe la categoría con id {dto.CategoriaId}.");
        }

        if (await _productos.ExisteCodigoAsync(codigo))
        {
            throw new ConflictException($"Ya existe un producto con el código '{codigo}'.");
        }

        var producto = new Producto
        {
            Codigo = codigo,
            Nombre = dto.Nombre.Trim(),
            Descripcion = dto.Descripcion?.Trim(),
            Precio = dto.Precio,
            Stock = dto.Stock,
            CategoriaId = dto.CategoriaId,
            FechaCreacion = DateTime.UtcNow
        };

        await _productos.AddAsync(producto);
        var creado = await _productos.GetByIdAsync(producto.Id);
        return creado!.ToDto();
    }

    public async Task<ProductoDto> ActualizarAsync(int id, ProductoUpdateDto dto)
    {
        var producto = await _productos.GetByIdAsync(id)
            ?? throw new NotFoundException($"No existe el producto con id {id}.");

        var codigo = dto.Codigo.Trim();

        if (await _categorias.GetByIdAsync(dto.CategoriaId) is null)
        {
            throw new NotFoundException($"No existe la categoría con id {dto.CategoriaId}.");
        }

        if (await _productos.ExisteCodigoAsync(codigo, id))
        {
            throw new ConflictException($"Ya existe un producto con el código '{codigo}'.");
        }

        producto.Codigo = codigo;
        producto.Nombre = dto.Nombre.Trim();
        producto.Descripcion = dto.Descripcion?.Trim();
        producto.Precio = dto.Precio;
        producto.CategoriaId = dto.CategoriaId;

        await _productos.UpdateAsync(producto);
        var actualizado = await _productos.GetByIdAsync(id);
        return actualizado!.ToDto();
    }

    public async Task EliminarAsync(int id)
    {
        var producto = await _productos.GetByIdAsync(id)
            ?? throw new NotFoundException($"No existe el producto con id {id}.");

        await _productos.DeleteAsync(producto);
    }
}
