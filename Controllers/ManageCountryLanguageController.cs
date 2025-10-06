using ABC.Data;
using ABC.Models.Domain;
using ABC.Models.DTO;
using ABC.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ABC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ManageCountryLanguageController : ControllerBase
    {
        private readonly ICountryLanguageRepository _repo;
        private readonly AbcDbContext dbContext;

        public ManageCountryLanguageController(ICountryLanguageRepository repo, AbcDbContext dbContext)
        {
            _repo = repo;
            this.dbContext = dbContext;
        }

        // Replace all occurrences of '_db' with 'dbContext' to match the injected field name

        [HttpGet("{countryId:guid}")]
        public async Task<IActionResult> GetByCountry(Guid countryId)
        {
            // ensure country exists
            var countryExists = await dbContext.Countries.AnyAsync(c => c.Id == countryId);
            if (!countryExists) return NotFound($"Country {countryId} not found.");

            var items = await _repo.GetByCountryIdWithLanguageAsync(countryId);

            var result = items.Select(cl => new CountryLanguageReadDto(
                cl.Id,
                cl.CountryId,
                cl.MultiSelectId,
                cl.Language?.Name ?? string.Empty,
                cl.Language?.DisplayName ?? string.Empty,
                cl.IsPrimary
            ));

            return Ok(result);
        }

        /// <summary>
        /// Add languages to a country (append). Duplicates are ignored.
        /// POST api/ManageCountryLanguage
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CountryLanguageCreateDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            // validate country existence
            var country = await dbContext.Countries.FindAsync(dto.CountryId);
            if (country is null) return NotFound($"Country {dto.CountryId} not found.");

            // load valid language MultiSelect entries (SelectionType == "Language")
            var validLanguages = await dbContext.MultiSelects
                .Where(ms => dto.LanguageIds.Contains(ms.Id) && ms.SelectionType == "Language" && ms.IsActive)
                .ToListAsync();

            if (!validLanguages.Any())
                return BadRequest("No valid language ids supplied.");

            var toAdd = new List<CountryLanguage>();
            foreach (var ms in validLanguages)
            {
                if (!await _repo.ExistsAsync(dto.CountryId, ms.Id))
                {
                    var cl = new CountryLanguage
                    {
                        CountryId = dto.CountryId,
                        MultiSelectId = ms.Id,
                        IsPrimary = dto.PrimaryLanguageId.HasValue && dto.PrimaryLanguageId.Value == ms.Id
                    };
                    toAdd.Add(cl);
                }
            }

            if (toAdd.Any())
            {
                await _repo.AddRangeAsync(toAdd);
                await _repo.SaveChangesAsync();
            }

            // return the newly added list (or current list)
            var current = await _repo.GetByCountryIdWithLanguageAsync(dto.CountryId);
            var read = current.Select(cl => new CountryLanguageReadDto(
                cl.Id, cl.CountryId, cl.MultiSelectId, cl.Language?.Name ?? "", cl.Language?.DisplayName ?? "", cl.IsPrimary
            ));

            return CreatedAtAction(nameof(GetByCountry), new { countryId = dto.CountryId }, read);
        }

        /// <summary>
        /// Replace the set of languages for a country (update)
        /// PUT api/ManageCountryLanguage/{countryId}
        /// </summary>
        [HttpPut("{countryId:guid}")]
        public async Task<IActionResult> Update(Guid countryId, [FromBody] CountryLanguageUpdateDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            if (dto.CountryId != countryId) return BadRequest("CountryId mismatch.");

            // validate country
            var country = await dbContext.Countries.FindAsync(countryId);
            if (country is null) return NotFound($"Country {countryId} not found.");

            // get current mappings
            var existing = (await _repo.GetByCountryIdAsync(countryId)).ToList();

            // remove all existing
            if (existing.Any())
            {
                await _repo.RemoveRangeAsync(existing);
            }

            // validate languages
            var validLanguages = await dbContext.MultiSelects
                .Where(ms => dto.LanguageIds.Contains(ms.Id) && ms.SelectionType == "Language" && ms.IsActive)
                .ToListAsync();

            var newLinks = validLanguages.Select(ms => new CountryLanguage
            {
                CountryId = countryId,
                MultiSelectId = ms.Id,
                IsPrimary = dto.PrimaryLanguageId.HasValue && dto.PrimaryLanguageId.Value == ms.Id
            }).ToList();

            if (newLinks.Any())
            {
                await _repo.AddRangeAsync(newLinks);
            }

            await _repo.SaveChangesAsync();

            var current = await _repo.GetByCountryIdWithLanguageAsync(countryId);
            var read = current.Select(cl => new CountryLanguageReadDto(
                cl.Id, cl.CountryId, cl.MultiSelectId, cl.Language?.Name ?? "", cl.Language?.DisplayName ?? "", cl.IsPrimary
            ));

            return Ok(read);
        }
        [HttpGet("countries")]
        public async Task<IActionResult> GetCountry()
        {
            // ensure country exists
            var countries = await dbContext.Countries.ToListAsync();
            return Ok(countries);
        }
    }
}
