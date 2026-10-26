using InventarioService.Dtos;
using InventarioService.Exceptions;
using InventarioService.Mappings;
using InventarioService.Models;
using InventarioService.Repositories;

namespace InventarioService.Services;

public class CategoriaService : ICategoriaService
{
    private readonly ICategoriaRepository _repo;

    public CategoriaService(ICategoriaRepository repo)
    {
        _repo = repo;
    }

    public async Task<List<CategoriaDto>> ListarAsync()
    {
        var categorias = await _repo.GetAllAsync();
        return categorias.OrderBy(c => c.Id).Select(c => c.ToDto()).ToList();
    }

    public async Task<CategoriaDto> ObtenerAsync(int id)
    {
        var categoria = await _repo.GetByIdAsync(id)
            ?? throw new NotFoundException($"No existe la categoría con id {id}.");
        return categoria.ToDto();
    }

    public async Task<CategoriaDto> CrearAsync(CategoriaCreateDto dto)
    {
        if (await _repo.ExisteNombreAsync(dto.Nombre))
        {
            throw new ConflictException($"Ya existe una categoría llamada '{dto.Nombre.Trim()}'.");
        }

        var categoria = new Categoria
        {
            Nombre = dto.Nombre.Trim(),
            Descripcion = dto.Descripcion?.Trim()
        };

        await _repo.AddAsync(categoria);
        return categoria.ToDto();
    }

    public async Task<CategoriaDto> ActualizarAsync(int id, CategoriaUpdateDto dto)
    {
        var categoria = await _repo.GetByIdAsync(id)
            ?? throw new NotFoundException($"No existe la categoría con id {id}.");

        if (await _repo.ExisteNombreAsync(dto.Nombre, id))
        {
            throw new ConflictException($"Ya existe una categoría llamada '{dto.Nombre.Trim()}'.");
        }

        categoria.Nombre = dto.Nombre.Trim();
        categoria.Descripcion = dto.Descripcion?.Trim();

        await _repo.UpdateAsync(categoria);
        return categoria.ToDto();
    }

    public async Task EliminarAsync(int id)
    {
        var categoria = await _repo.GetByIdAsync(id)
            ?? throw new NotFoundException($"No existe la categoría con id {id}.");

        if (await _repo.TieneProductosAsync(id))
        {
            throw new ConflictException("No se puede eliminar una categoría que tiene productos asociados.");
        }

        await _repo.DeleteAsync(categoria);
    }
}
