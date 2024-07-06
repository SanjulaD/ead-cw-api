using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityStudentTracker.API.Models.DTO.Auth;
using UniversityStudentTracker.API.Models.DTO.User;
using UniversityStudentTracker.API.Utils.Enums;

namespace UniversityStudentTracker.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = nameof(UserRole.Admin))]
public class AdminController : ControllerBase
{
    private readonly ILogger<AdminController> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<IdentityUser> _userManager;

    public AdminController(UserManager<IdentityUser> userManager, IMapper mapper, ILogger<AdminController> logger)
    {
        _userManager = userManager;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpPost]
    [Route("Create")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
    {
        _logger.LogInformation("User was registered successfully.");
        return Ok("User was registered! Please Login");
    }

    [HttpDelete]
    [Route("Delete/{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user == null)
        {
            _logger.LogWarning("User not found with id: {UserId}", id);
            return NotFound("User not found");
        }

        var result = await _userManager.DeleteAsync(user);

        if (result.Succeeded)
        {
            _logger.LogInformation("User deleted successfully with id: {UserId}", id);
            return Ok("User deleted successfully");
        }

        _logger.LogError("Error occurred while deleting user with id: {UserId}", id);
        return BadRequest("Something went wrong while deleting the user");
    }

    [HttpGet]
    [Route("Users")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userManager.Users.ToListAsync();

        var userDtos = new List<UserDto>();

        foreach (var user in users)
        {
            var userDto = _mapper.Map<UserDto>(user);
            userDto.Roles = (await _userManager.GetRolesAsync(user)).ToList();
            userDtos.Add(userDto);
        }

        _logger.LogInformation("Fetched {UserCount} users.", users.Count);

        return Ok(userDtos);
    }
}