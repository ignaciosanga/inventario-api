using System.ComponentModel.DataAnnotations;
using InventarioService.Models;

namespace InventarioService.Dtos;

public class MovimientoCreateDto
{
    [Range(1, int.MaxValue)]
    public int ProductoId { get; set; }

    [EnumDataType(typeof(TipoMovimiento))]
    public TipoMovimiento Tipo { get; set; }

    [Range(1, int.MaxValue)]
    public int Cantidad { get; set; }

    [StringLength(300)]
    public string? Motivo { get; set; }
}

public class MovimientoDto
{
    public int Id { get; set; }
    public int ProductoId { get; set; }
    public string? ProductoNombre { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public string? Motivo { get; set; }
    public DateTime Fecha { get; set; }
}
