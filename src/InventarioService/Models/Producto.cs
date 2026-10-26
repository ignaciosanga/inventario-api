namespace InventarioService.Models;

public class Producto
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public decimal Precio { get; set; }
    public int Stock { get; set; }
    public int CategoriaId { get; set; }
    public Categoria? Categoria { get; set; }
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
    public List<Movimiento> Movimientos { get; set; } = new();
}
