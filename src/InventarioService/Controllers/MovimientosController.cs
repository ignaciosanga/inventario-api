using InventarioService.Dtos;
using InventarioService.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventarioService.Controllers;

[ApiController]
[Route("api/movimientos")]
public class MovimientosController : ControllerBase
{
    private readonly IMovimientoService _service;

    public MovimientosController(IMovimientoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<MovimientoDto>>> Listar()
    {
        return Ok(await _service.ListarAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MovimientoDto>> Obtener(int id)
    {
        return Ok(await _service.ObtenerAsync(id));
    }

    [HttpPost]
    public async Task<ActionResult<MovimientoDto>> Registrar(MovimientoCreateDto dto)
    {
        var creado = await _service.RegistrarAsync(dto);
        return CreatedAtAction(nameof(Obtener), new { id = creado.Id }, creado);
    }
}
