using ABC.Data;
using ABC.Models.Domain;
using ABC.Models.DTO;
using ABC.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ABC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatesController : ControllerBase
    {
        private readonly IRateRepository _rateRepo;
        private readonly AbcDbContext _context;

        public RatesController(IRateRepository rateRepo, AbcDbContext context)
        {
            _rateRepo = rateRepo ?? throw new ArgumentNullException(nameof(rateRepo));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // GET /api/rates/{entityType}/{entityId}?asOf=yyyy-MM-dd
        [HttpGet("{entityType}/{entityId:guid}")]
        public async Task<IActionResult> GetRates(string entityType, Guid entityId, [FromQuery] DateTime? asOf = null)
        {
            entityType = (entityType ?? string.Empty).Trim();

            if (asOf.HasValue)
            {
                var dto = await _rateRepo.GetRateAsOfAsync(entityType, entityId, asOf.Value.Date);
                if (dto == null) return NotFound();
                return Ok(dto);
            }

            var active = await _rateRepo.GetActiveRateAsync(entityType, entityId, DateTime.UtcNow.Date);
            var history = await _rateRepo.GetHistoryAsync(entityType, entityId);

            // Map to DTOs (simple inline mapping)
            RateDto MapToDto(RateHistory r) =>
                r == null ? null : new RateDto
                {
                    Id = r.Id,
                    EntityType = r.EntityType,
                    EntityId = r.EntityId,
                    Rate = r.Rate,
                    Currency = r.Currency,
                    StartDate = r.StartDate,
                    EndDate = r.EndDate,
                    Note = r.Note,
                    CreatedAt = r.CreatedAt
                };

            return Ok(new
            {
                activeRate = MapToDto(active),
                history = history?.Select(MapToDto) ?? Array.Empty<RateDto>()
            });
        }

        // POST /api/rates/{entityType}/{entityId}
        [HttpPost("{entityType}/{entityId:guid}")]
        public async Task<IActionResult> CreateRate(string entityType, Guid entityId, [FromBody] CreateRateRequest request)
        {
            if (request == null) return BadRequest("Request body required.");
            if (request.Rate < 0) return BadRequest("Rate must be >= 0.");

            entityType = (entityType ?? string.Empty).Trim();
            var newStart = DateTime.UtcNow;

            // Transactional update: close overlapping previous rates and insert new rate
            using (var tx = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Find overlapping previous records
                    var overlapping = await _context.RateHistory
                        .Where(r => r.EntityType == entityType && r.EntityId == entityId
                                    && r.StartDate < newStart
                                    && (r.EndDate == null || r.EndDate >= newStart))
                        .ToListAsync();

                    foreach (var prev in overlapping)
                    {
                        prev.EndDate = DateTime.UtcNow;
                        _context.RateHistory.Update(prev);
                    }

                    // Insert new rate
                    var newRate = new RateHistory
                    {
                        Id = Guid.NewGuid(),
                        EntityType = entityType,
                        EntityId = entityId,
                        Rate = request.Rate,
                        Currency = request.Currency,
                        StartDate = newStart,
                        EndDate = null,
                        Note = request.Note,
                        CreatedBy = null,
                        CreatedAt = DateTime.UtcNow
                    };

                    await _rateRepo.AddAsync(newRate);
                    await _rateRepo.SaveChangesAsync();

                    await tx.CommitAsync();

                    // Map to DTO for response
                    var createdDto = new RateDto
                    {
                        Id = newRate.Id,
                        EntityType = newRate.EntityType,
                        EntityId = newRate.EntityId,
                        Rate = newRate.Rate,
                        Currency = newRate.Currency,
                        StartDate = newRate.StartDate,
                        EndDate = newRate.EndDate,
                        Note = newRate.Note,
                        CreatedAt = newRate.CreatedAt
                    };

                    return CreatedAtAction(nameof(GetRates), new { entityType, entityId }, createdDto);
                }
                catch (Exception ex)
                {
                    await tx.RollbackAsync();
                    // log ex if you have logger
                    return StatusCode(500, "Failed to create rate: " + ex.Message);
                }
            }
        }
    }
}
