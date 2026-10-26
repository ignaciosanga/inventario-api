namespace InventarioService.Models;

public enum TipoMovimiento
{
    Entrada = 1,
    Salida = 2
}

public class Movimiento
{
    public int Id { get; set; }
    public int ProductoId { get; set; }
    public Producto? Producto { get; set; }
    public TipoMovimiento Tipo { get; set; }
    public int Cantidad { get; set; }
    public string? Motivo { get; set; }
    public DateTime Fecha { get; set; } = DateTime.UtcNow;
}
