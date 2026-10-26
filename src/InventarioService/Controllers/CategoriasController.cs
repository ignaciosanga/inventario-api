using InventarioService.Dtos;
using InventarioService.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventarioService.Controllers;

[ApiController]
[Route("api/categorias")]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaService _service;

    public CategoriasController(ICategoriaService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoriaDto>>> Listar()
    {
        return Ok(await _service.ListarAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CategoriaDto>> Obtener(int id)
    {
        return Ok(await _service.ObtenerAsync(id));
    }

    [HttpPost]
    public async Task<ActionResult<CategoriaDto>> Crear(CategoriaCreateDto dto)
    {
        var creada = await _service.CrearAsync(dto);
        return CreatedAtAction(nameof(Obtener), new { id = creada.Id }, creada);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<CategoriaDto>> Actualizar(int id, CategoriaUpdateDto dto)
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
