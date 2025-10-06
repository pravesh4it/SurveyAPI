using ABC.Models.Domain;
using ABC.Models.DTO;
using ABC.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ABC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountryController : ControllerBase
    {
        private readonly ICountryRepository _repo;
        public CountryController(ICountryRepository repo) => _repo = repo;

        /// <summary>
        /// GET /api/Country
        /// list all countries
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var items = await _repo.GetAllAsync();
            var dto = items.Select(c => new CountryReadDto(
                c.Id, c.Name, c.Currency, c.CurrencySymbol, c.IsdCode, c.ShortCode
            ));
            return Ok(dto);
        }

        /// <summary>
        /// GET /api/Country/{id}
        /// get by id
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var c = await _repo.GetByIdAsync(id);
            if (c == null) return NotFound();
            var dto = new CountryReadDto(c.Id, c.Name, c.Currency, c.CurrencySymbol, c.IsdCode, c.ShortCode);
            return Ok(dto);
        }

        /// <summary>
        /// POST /api/Country
        /// create
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CountryCreateDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var entity = new Country
            {
                Id = Guid.NewGuid(),
                Name = dto.Name.Trim(),
                Currency = dto.Currency.Trim(),
                CurrencySymbol = dto.CurrencySymbol.Trim(),
                IsdCode = dto.IsdCode.Trim(),
                ShortCode = dto.ShortCode.Trim()
            };

            var created = await _repo.AddAsync(entity);
            var read = new CountryReadDto(created.Id, created.Name, created.Currency, created.CurrencySymbol, created.IsdCode, created.ShortCode);
            return CreatedAtAction(nameof(GetById), new { id = read.Id }, read);
        }

        /// <summary>
        /// PUT /api/Country/{id}
        /// update
        /// </summary>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CountryUpdateDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var exists = await _repo.ExistsAsync(id);
            if (!exists) return NotFound();

            var entity = new Country
            {
                Id = id,
                Name = dto.Name.Trim(),
                Currency = dto.Currency.Trim(),
                CurrencySymbol = dto.CurrencySymbol.Trim(),
                IsdCode = dto.IsdCode.Trim(),
                ShortCode = dto.ShortCode.Trim()
            };

            var ok = await _repo.UpdateAsync(entity);
            if (!ok) return NotFound();
            return NoContent();
        }

        /// <summary>
        /// DELETE /api/Country/{id}
        /// </summary>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var ok = await _repo.DeleteAsync(id);
            return ok ? NoContent() : NotFound();
        }
    }
}
