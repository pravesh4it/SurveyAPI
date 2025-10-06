using ABC.Data;
using ABC.Models.Domain;
using ABC.Models.DTO;
using ABC.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ABC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ItokenRepository tokenRepository;
        private readonly IUsersRepository usersRepository;
        private readonly AbcDbContext dbContext;
        private readonly IEmailService _emailService;

        public AuthController(UserManager<IdentityUser> userManager, ItokenRepository tokenRepository, IUsersRepository usersRepository, AbcDbContext dbContext, IEmailService emailService)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
            this.usersRepository = usersRepository;
            this.dbContext = dbContext;
            _emailService = emailService;
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Username
            };
            var identityResult = await userManager.CreateAsync(identityUser);
            if (identityResult.Succeeded)
            {
                if (registerRequestDto.Role != null)
                {
                    string[] Roles = registerRequestDto.Role.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    identityResult = await userManager.AddToRolesAsync(identityUser, Roles);
                    if (identityResult.Succeeded)
                    {
                        // add firstname, last name, designation 
                        var userresponse = await usersRepository.AddUserAsync(registerRequestDto, identityUser.Id);
                        // SEND EMAIL TO ACTIVATE USER ON EMAIL ADDRESS
                        var mailqueue = await usersRepository.AddEmailRegisterAsync(registerRequestDto.Username, identityUser.Id);
                        return Ok("user was registered");
                    }
                }
            }
            else
            {
                // Log or display each error message
                foreach (var error in identityResult.Errors)
                {
                    Console.WriteLine($"Error Code: {error.Code}, Description: {error.Description}");
                }
                // Optionally, you can log this information or add to a list to display in the UI
                var errorMessages = identityResult.Errors.Select(e => e.Description).ToList();

                // Example: Returning the error messages to the caller
                return BadRequest(new { Errors = errorMessages });

            }
            return BadRequest("something went wrong");
        }

        [HttpPost]
        // post :// api/auth/login
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDto.Username);
            if (user != null)
            {
                var checkPasswordResult = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);
                if (checkPasswordResult)
                {
                    var roles = await userManager.GetRolesAsync(user);
                    if (roles.Any())
                    {
                        var jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList());
                        var response = new LoginResponseDto
                        {
                            JwtToken = jwtToken,
                            Email = user.Email,
                            Id = user.Id,
                            Role = String.Join(", ", roles.ToList())

                        };
                        return Ok(response);

                    }
                }
            }
            return BadRequest("something went wrong");
        }

        [HttpDelete("DeleteUser/{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            try
            {
                // Find the user in Identity
                var identityUser = await userManager.FindByIdAsync(userId);
                if (identityUser == null)
                {
                    return NotFound("User not found.");
                }

                // Find the user in UserInfo table
                var userInfo = await dbContext.UserInfoes.FirstOrDefaultAsync(u => u.AspNetUsersId == userId);
                if (userInfo != null)
                {
                    dbContext.UserInfoes.Remove(userInfo);
                }

                // Delete the user from Identity
                var result = await userManager.DeleteAsync(identityUser);
                if (!result.Succeeded)
                {
                    return BadRequest(new { Errors = result.Errors.Select(e => e.Description).ToList() });
                }

                // Save changes
                await dbContext.SaveChangesAsync();

                return Ok("User deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("register-details")]
        public async Task<IActionResult> RegisterDetails([FromBody] RegisterUserDto dto)
        {
            string[] role = { "ReadOnly" };
            var identityUser = new IdentityUser
            {
                UserName = dto.PersonalInfo.Email,
                Email = dto.PersonalInfo.Email
            };
            var identityResult = await userManager.CreateAsync(identityUser);
            if (identityResult.Succeeded)
            {
                identityResult = await userManager.AddToRolesAsync(identityUser, role);
                if (identityResult.Succeeded)
                {
                    // add firstname, last name, designation 
                    var userresponse = await usersRepository.AddUserProfileAsync(dto, identityUser.Id);
                    // SEND EMAIL TO ACTIVATE USER ON EMAIL ADDRESS
                    MailQueue mailqueue = await usersRepository.AddEmailRegisterUserAsync(dto.PersonalInfo.Email, identityUser.Id);
                    //Send email using the email service
                    try
                    {
                        await _emailService.SendEmailAsync(mailqueue.ToMail, mailqueue.Subject, mailqueue.content);
                    }
                    catch (Exception ex)
                    {
                        // Handle email sending failure
                        Console.WriteLine($"Failed to send email: {ex.Message}");
                        //return StatusCode(StatusCodes.Status500InternalServerError, "Failed to send activation email.");
                    }
                    return Ok("Registration Successful");
                }
            }
            else
            {
                // Log or display each error message
                foreach (var error in identityResult.Errors)
                {
                    Console.WriteLine($"Error Code: {error.Code}, Description: {error.Description}");
                }
                // Optionally, you can log this information or add to a list to display in the UI
                var errorMessages = identityResult.Errors.Select(e => e.Description).ToList();

                // Example: Returning the error messages to the caller
                return BadRequest(new { Errors = errorMessages });

            }
            return BadRequest("something went wrong");
        }
        [HttpGet("get-email-by-code/{code}")]
        public async Task<IActionResult> GetUserEmail(string code)
        {
            try
            {
                // Find the user in UserProfiles table using the reset code
                var userInfo = await dbContext.UserProfiles.FirstOrDefaultAsync(u => u.PasswordResetCode == code);

                if (userInfo != null)
                {
                    // Get the corresponding ASP.NET Identity user
                    var aspnetuser = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userInfo.UserId);

                    if (aspnetuser != null)
                    {
                        // Return the email
                        return Ok(new { email = aspnetuser.Email });
                    }

                    return NotFound("User not found in ASP.NET Identity.");
                }

                return NotFound("Invalid or expired reset code.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto request)
        {
            if (string.IsNullOrEmpty(request.Code) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Reset code and password are required.");
            }

            try
            {
                // Find user profile by reset code
                var userProfile = await dbContext.UserProfiles
                    .FirstOrDefaultAsync(u => u.PasswordResetCode == request.Code);

                if (userProfile == null)
                {
                    return NotFound("Invalid or expired reset code.");
                }

                // Get user from Identity table
                var user = await userManager.FindByIdAsync(userProfile.UserId.ToString());
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                // Remove old password (optional: if not using password reset token)
                var token = await userManager.GeneratePasswordResetTokenAsync(user);

                var result = await userManager.ResetPasswordAsync(user, token, request.Password);

                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description);
                    return BadRequest(new { message = "Password reset failed.", errors });
                }

                // Clear the reset code after successful reset
                userProfile.PasswordResetCode = null;
                await dbContext.SaveChangesAsync();

                return Ok(new { message = "Password reset successful." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("send-mail")]
        public async Task<IActionResult> SendMail([FromBody] MailSendDto mailSendDto)
        {
            try
            {
                await _emailService.SendEmailAsync(mailSendDto.ToEmail, mailSendDto.Subject, mailSendDto.Body);
                return Ok(new { message = "mail sent successful." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        // post :// api/auth/login
        [Route("login-reward")]
        public async Task<IActionResult> LoginReward([FromBody] LoginRequestDto loginRequestDto)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDto.Username);
            if (user != null)
            {
                var checkPasswordResult = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);
                if (checkPasswordResult)
                {
                    var roles = await userManager.GetRolesAsync(user);
                    if (roles.Any())
                    {
                        var jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList());
                        UserProfile? userProfile = await dbContext.UserProfiles.FirstOrDefaultAsync(u => u.UserId == user.Id);
                        var response = new LoginResponseDto
                        {
                            JwtToken = jwtToken,
                            Email = user.Email,
                            Id = user.Id,
                            Role = String.Join(", ", roles.ToList()),
                            Name = userProfile != null ? userProfile.FullName : user.UserName // fallback to UserName
                        };
                        return Ok(response);

                    }
                }
            }
            return BadRequest("something went wrong");
        }
        [HttpPost]
        // post :// api/auth/login
        [Route("change-password")]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] PasswordChangeRequestDto passwordChangeRequestDto)
        {
            var user = await userManager.FindByIdAsync(passwordChangeRequestDto.Id) ?? throw new Exception("User not found");
            var result = await userManager.ChangePasswordAsync(user, passwordChangeRequestDto.OldPassword, passwordChangeRequestDto.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(string.Join("; ", result.Errors.Select(e => e.Description)));
            }
            return Ok(true);
        }
    }
}
