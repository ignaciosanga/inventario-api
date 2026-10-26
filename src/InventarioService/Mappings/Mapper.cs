using InventarioService.Dtos;
using InventarioService.Models;

namespace InventarioService.Mappings;

public static class Mapper
{
    public static CategoriaDto ToDto(this Categoria c)
    {
        return new CategoriaDto
        {
            Id = c.Id,
            Nombre = c.Nombre,
            Descripcion = c.Descripcion
        };
    }

    public static ProductoDto ToDto(this Producto p)
    {
        return new ProductoDto
        {
            Id = p.Id,
            Codigo = p.Codigo,
            Nombre = p.Nombre,
            Descripcion = p.Descripcion,
            Precio = p.Precio,
            Stock = p.Stock,
            CategoriaId = p.CategoriaId,
            CategoriaNombre = p.Categoria?.Nombre,
            FechaCreacion = p.FechaCreacion
        };
    }

    public static MovimientoDto ToDto(this Movimiento m)
    {
        return new MovimientoDto
        {
            Id = m.Id,
            ProductoId = m.ProductoId,
            ProductoNombre = m.Producto?.Nombre,
            Tipo = m.Tipo.ToString(),
            Cantidad = m.Cantidad,
            Motivo = m.Motivo,
            Fecha = m.Fecha
        };
    }
}
