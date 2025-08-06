using ABC.Models.DTO;
using ABC.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ABC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        [Authorize(Roles ="SuperAdmin")]
        public async Task<IActionResult> GetAll()
        {
            var regionDomain = await regionRepository.GetAllAsync();
            var regionDto = mapper.Map<List<RegionDto>>(regionDomain);
            return Ok(regionDto);

        }


    }
}
