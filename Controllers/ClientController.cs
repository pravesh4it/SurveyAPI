using ABC.Models.Domain;
using ABC.Models.DTO;
using ABC.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ABC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientRepository clientRepository;
        private readonly IMapper mapper;
        private readonly ClientSetting clientSetting;

        public ClientController(IClientRepository clientRepository, IMapper mapper, IOptions<ClientSetting> clientSettingOptions)
        {
            this.clientRepository = clientRepository;
            this.mapper = mapper;
            this.clientSetting = clientSettingOptions.Value;
        }
        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                string clientTypeId = clientSetting.Client;
                var clientDomain = await clientRepository.GetAllAsync(clientTypeId);
                var clientDto = mapper.Map<List<ClientDto>>(clientDomain);
                return Ok(clientDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/Client
        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Create([FromBody] ClientDto clientDto)
        {
            try
            {
                if (clientDto == null)
                    return BadRequest("Client object is null");

                if (!ModelState.IsValid)
                    return BadRequest("Invalid model object");


                clientDto.Id = new Guid();
                clientDto.ClientTypeId = clientSetting.Client;
                clientDto.CreatedOn = DateTime.Now;
                var clientDomain = mapper.Map<Client>(clientDto);
                var createdClient = await clientRepository.CreateAsync(clientDomain);
                var createdClientDto = mapper.Map<ClientDto>(createdClient);

                return CreatedAtAction(nameof(GetAll), new { id = createdClientDto.Id }, createdClientDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/Client/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ClientDto clientDto)
        {
            try
            {
                if (clientDto == null || id != clientDto.Id)
                    return BadRequest("Client object is null or IDs do not match");

                if (!ModelState.IsValid)
                    return BadRequest("Invalid model object");

                clientDto.ClientTypeId = clientSetting.Client;
                var clientDomain = mapper.Map<Client>(clientDto);
                var updatedClient = await clientRepository.UpdateAsync(clientDomain);

                if (updatedClient == null)
                    return NotFound("Client not found");

                var updatedClientDto = mapper.Map<ClientDto>(updatedClient);

                return Ok(updatedClientDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/Client/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetClient(Guid id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid model object");

                var clientDomain = await clientRepository.GetClentAsync(id);
                return Ok(mapper.Map<ClientDto>(clientDomain));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/Client/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var deleted = await clientRepository.DeleteAsync(id);
                if (!deleted)
                    return NotFound("Client not found");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
