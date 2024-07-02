using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UniversityStudentTracker.API.Models.DTO.Auth;
using UniversityStudentTracker.API.Repositories;

namespace UniversityStudentTracker.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthRepository _authRepository;
    private readonly UserManager<IdentityUser> _userManager;

    public AuthController(UserManager<IdentityUser> userManager, IAuthRepository authRepository)
    {
        _userManager = userManager;
        _authRepository = authRepository;
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
        var identityResult = await _userManager.CreateAsync(identityUser, registerRequestDto.Password);

        if (identityResult.Succeeded)
            if (registerRequestDto.Roles.Any())
            {
                identityResult = await _userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);

                if (identityResult.Succeeded) return Ok("User was registered! Please Login");
            }

        return BadRequest("Something went wrong");
    }

    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
    {
        var user = await _userManager.FindByEmailAsync(loginRequestDto.Username);
        if (user == null) return BadRequest("Username or password incorrect");
        var checkPasswordResult = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

        if (!checkPasswordResult) return BadRequest("Username or password incorrect");

        var roles = await _userManager.GetRolesAsync(user);
        var jwtToken = _authRepository.CreateJwtToken(user, roles.ToList());

        var response = new LoginResponseDto
        {
            JwtToken = jwtToken,
            Username = user.UserName,
            Email = user.Email,
            UserId = user.Id,
            Role = roles.FirstOrDefault()
        };

        return Ok(response);
    }
}