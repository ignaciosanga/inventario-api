using InventarioService.Dtos;
using InventarioService.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventarioService.Controllers;

[ApiController]
[Route("api/productos")]
public class ProductosController : ControllerBase
{
    private readonly IProductoService _service;
    private readonly IMovimientoService _movimientos;

    public ProductosController(IProductoService service, IMovimientoService movimientos)
    {
        _service = service;
        _movimientos = movimientos;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductoDto>>> Listar()
    {
        return Ok(await _service.ListarAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductoDto>> Obtener(int id)
    {
        return Ok(await _service.ObtenerAsync(id));
    }

    [HttpGet("{id:int}/movimientos")]
    public async Task<ActionResult<List<MovimientoDto>>> ListarMovimientos(int id)
    {
        return Ok(await _movimientos.ListarPorProductoAsync(id));
    }

    [HttpPost]
    public async Task<ActionResult<ProductoDto>> Crear(ProductoCreateDto dto)
    {
        var creado = await _service.CrearAsync(dto);
        return CreatedAtAction(nameof(Obtener), new { id = creado.Id }, creado);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProductoDto>> Actualizar(int id, ProductoUpdateDto dto)
    {
        return Ok(await _service.ActualizarAsync(id, dto));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        await _service.EliminarAsync(id);
        return NoContent();
    }
}
