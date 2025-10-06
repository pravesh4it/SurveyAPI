using ABC.Models.Domain;
using ABC.Models.DTO;
using ABC.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ABC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MultiselectController : ControllerBase
    {
        private readonly IMultiselectRepository _repo;
        public MultiselectController(IMultiselectRepository repo) => _repo = repo;


        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string selectionType)
        {
            if (string.IsNullOrWhiteSpace(selectionType)) return BadRequest("selectionType is required");
            var items = await _repo.GetBySelectionTypeAsync(selectionType);
            var result = items.Select(MapToReadDto);
            return Ok(result);
        }


        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var item = await _repo.GetByIdAsync(id);
            return item is null ? NotFound() : Ok(MapToReadDto(item));
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MultiselectCreateDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var entity = new MultiSelect
            {
                Name = dto.Name.Trim(),
                DisplayName = dto.DisplayName.Trim(),
                IsActive = dto.IsActive,
                Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description!.Trim(),
                SelectionType = dto.SelectionType.Trim(),
            };
            var created = await _repo.AddAsync(entity);
            var read = MapToReadDto(created);
            return CreatedAtAction(nameof(GetById), new { id = read.Id }, read);
        }


        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] MultiselectUpdateDto dto)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing is null) return NotFound();


            existing.Name = dto.Name.Trim();
            existing.DisplayName = dto.DisplayName.Trim();
            existing.IsActive = dto.IsActive;
            existing.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description!.Trim();
            existing.SelectionType = dto.SelectionType.Trim();


            await _repo.UpdateAsync(existing);
            return NoContent();
        }


        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var ok = await _repo.DeleteAsync(id);
            return ok ? NoContent() : NotFound();
        }


        private static MultiselectReadDto MapToReadDto(MultiSelect e) => new(
        e.Id, e.Name, e.DisplayName, e.IsActive, e.Description, e.SelectionType
        );
    }
}
