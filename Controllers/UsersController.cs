using ABC.Models.Domain;
using ABC.Models.DTO;
using ABC.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ABC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IMapper mapper;
        public UsersController(IUsersRepository usersRepository, IMapper mapper)
        {
            _usersRepository = usersRepository;
            this.mapper = mapper;
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult> GetUsers()
        {
            return Ok(await _usersRepository.GetAllUsersAsync());
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UserInfo>> GetUserById(string id)
        {
            var user = await _usersRepository.GetUserByIdAsync(id);
            if (user == null) return NotFound();

            return Ok(user);
        }

        // PUT: api/users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, UserInfo user)
        {
            if (id != user.AspNetUsersId) return BadRequest();

            await _usersRepository.UpdateUserAsync(user);
            return NoContent();
        }

        // GET: api/users
        [HttpGet("options")]
        public async Task<ActionResult> GetOptions()
        {
            UserOptionsDto userOptionsDto = new UserOptionsDto();
            userOptionsDto = await _usersRepository.Getoptions();

            return Ok(userOptionsDto);
        }
        [HttpGet("user-profile/{user_id}")]
        public async Task<IActionResult> GetUserProfile(string user_id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid model object");

                var userProfile = await _usersRepository.GetUserProfileAsync(user_id);
                return Ok(userProfile);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("admin-profile/{user_id}")]
        public async Task<IActionResult> GetAdminProfile(string user_id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid model object");

                var userProfile = await _usersRepository.GetAdminProfileAsync(user_id);
                return Ok(userProfile);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    }
}
