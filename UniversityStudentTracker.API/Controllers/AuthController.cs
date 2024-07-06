using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UniversityStudentTracker.API.Models.DTO.Auth;
using UniversityStudentTracker.API.Repositories;
using UniversityStudentTracker.API.Utils.Enums;

namespace UniversityStudentTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthInterface _authInterface;
        private readonly ILogger<AuthController> _logger;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthController(UserManager<IdentityUser> userManager,
            IAuthInterface authInterface,
            ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _authInterface = authInterface;
            _logger = logger;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            _logger.LogInformation("Register endpoint called.");

            var identityUser = new IdentityUser
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Username
            };
            var identityResult = await _userManager.CreateAsync(identityUser, registerRequestDto.Password);

            if (!identityResult.Succeeded)
            {
                _logger.LogWarning("User registration failed.");
                return BadRequest("Something went wrong");
            }

            identityResult = await _userManager.AddToRoleAsync(identityUser, nameof(UserRole.Student));

            if (identityResult.Succeeded)
            {
                _logger.LogInformation("User registered successfully.");
                return Ok("User was registered! Please Login");
            }
            else
            {
                _logger.LogWarning("Failed to assign role to the user.");
                return BadRequest("Something went wrong");
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var user = await _userManager.FindByEmailAsync(loginRequestDto.Username);
            if (user == null)
            {
                _logger.LogWarning("User not found with email {Email}.", loginRequestDto.Username);
                return BadRequest("Username or password incorrect");
            }

            var checkPasswordResult = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

            if (!checkPasswordResult)
            {
                _logger.LogWarning("Incorrect password for user {Username}.", user.UserName);
                return BadRequest("Username or password incorrect");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var jwtToken = _authInterface.CreateJwtToken(user, roles.ToList());

            var response = new LoginResponseDto
            {
                JwtToken = jwtToken,
                Username = user.UserName,
                Email = user.Email,
                UserId = user.Id,
                Role = roles.FirstOrDefault()
            };

            _logger.LogInformation("User {Username} logged in successfully.", user.UserName);

            return Ok(response);
        }
    }
}