using InventarioService.Data;
using InventarioService.Dtos;
using InventarioService.Exceptions;
using InventarioService.Mappings;
using InventarioService.Models;
using InventarioService.Repositories;

namespace InventarioService.Services;

public class MovimientoService : IMovimientoService
{
    private readonly IMovimientoRepository _movimientos;
    private readonly IProductoRepository _productos;
    private readonly AppDbContext _context;

    public MovimientoService(IMovimientoRepository movimientos, IProductoRepository productos, AppDbContext context)
    {
        _movimientos = movimientos;
        _productos = productos;
        _context = context;
    }

    public async Task<List<MovimientoDto>> ListarAsync()
    {
        var lista = await _movimientos.GetAllAsync();
        return lista.Select(m => m.ToDto()).ToList();
    }

    public async Task<List<MovimientoDto>> ListarPorProductoAsync(int productoId)
    {
        if (await _productos.GetByIdAsync(productoId) is null)
        {
            throw new NotFoundException($"No existe el producto con id {productoId}.");
        }

        var lista = await _movimientos.GetByProductoAsync(productoId);
        return lista.Select(m => m.ToDto()).ToList();
    }

    public async Task<MovimientoDto> ObtenerAsync(int id)
    {
        var movimiento = await _movimientos.GetByIdAsync(id)
            ?? throw new NotFoundException($"No existe el movimiento con id {id}.");
        return movimiento.ToDto();
    }

    public async Task<MovimientoDto> RegistrarAsync(MovimientoCreateDto dto)
    {
        var producto = await _productos.GetByIdAsync(dto.ProductoId)
            ?? throw new NotFoundException($"No existe el producto con id {dto.ProductoId}.");

        if (dto.Tipo == TipoMovimiento.Salida && producto.Stock < dto.Cantidad)
        {
            throw new BusinessRuleException(
                $"Stock insuficiente: hay {producto.Stock} unidades y se intentan sacar {dto.Cantidad}.");
        }

        await using var transaccion = await _context.Database.BeginTransactionAsync();

        producto.Stock += dto.Tipo == TipoMovimiento.Entrada ? dto.Cantidad : -dto.Cantidad;

        var movimiento = new Movimiento
        {
            ProductoId = producto.Id,
            Tipo = dto.Tipo,
            Cantidad = dto.Cantidad,
            Motivo = dto.Motivo?.Trim(),
            Fecha = DateTime.UtcNow
        };

        await _productos.UpdateAsync(producto);
        await _movimientos.AddAsync(movimiento);
        await transaccion.CommitAsync();

        var creado = await _movimientos.GetByIdAsync(movimiento.Id);
        return creado!.ToDto();
    }
}
