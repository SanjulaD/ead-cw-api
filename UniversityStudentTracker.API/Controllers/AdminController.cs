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
    private readonly IMapper _mapper;
    private readonly UserManager<IdentityUser> _userManager;

    public AdminController(UserManager<IdentityUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    [HttpPost]
    [Route("Create")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
    {
        var identityUser = new IdentityUser
        {
            UserName = registerRequestDto.Username,
            Email = registerRequestDto.Username
        };
        var identityResult = await _userManager.CreateAsync(identityUser, registerRequestDto.Password);

        if (!identityResult.Succeeded) return BadRequest("Something went wrong");
        identityResult = await _userManager.AddToRoleAsync(identityUser, nameof(UserRole.Admin));

        if (identityResult.Succeeded) return Ok("User was registered! Please Login");

        return BadRequest("Something went wrong");
    }

    [HttpDelete]
    [Route("Delete/{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user == null) return NotFound("User not found");

        var result = await _userManager.DeleteAsync(user);

        if (result.Succeeded) return Ok("User deleted successfully");

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

        return Ok(userDtos);
    }
}