using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityStudentTracker.API.Helpers;
using UniversityStudentTracker.API.Models.DTO.Auth;
using UniversityStudentTracker.API.Models.DTO.User;
using UniversityStudentTracker.API.Services;
using UniversityStudentTracker.API.Utils.Enums;

namespace UniversityStudentTracker.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = nameof(UserRole.Admin))]
public class AdminController : ControllerBase
{
    private readonly AdminService _adminService;
    private readonly ILogger<AdminController> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<IdentityUser> _userManager;

    public AdminController(UserManager<IdentityUser> userManager, IMapper mapper, ILogger<AdminController> logger,
        AdminService adminService)
    {
        _userManager = userManager;
        _mapper = mapper;
        _logger = logger;
        _adminService = adminService;
    }

    [HttpPost]
    [Route("Create")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
    {
        var admin = await _userManager.FindByEmailAsync(registerRequestDto.Username);
        if (admin != null)
        {
            _logger.LogWarning("Admin found with email {Email}.", registerRequestDto.Username);
            return BadRequest("Admin already exists! Try with different username");
        }

        var identityUser = new IdentityUser
        {
            UserName = registerRequestDto.Username,
            Email = registerRequestDto.Username
        };
        var identityResult = await _userManager.CreateAsync(identityUser, registerRequestDto.Password);

        if (!identityResult.Succeeded)
        {
            _logger.LogWarning("Admin registration failed.");
            return BadRequest("Something went wrong");
        }

        identityResult = await _userManager.AddToRoleAsync(identityUser, nameof(UserRole.Admin));

        if (identityResult.Succeeded)
        {
            _logger.LogInformation("Admin registered successfully.");
            return Ok("User was registered! Please Login");
        }

        _logger.LogWarning("Failed to assign role to the user.");
        return BadRequest("Something went wrong");
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

    [HttpGet]
    [Route("Metrics")]
    public async Task<IActionResult> GetAdminStatistics()
    {
        var currentDate = DateTime.UtcNow;

        // Get start and end of week
        var (startOfWeek, endOfWeek) = DateHelper.GetStartEndOfDay(currentDate.AddDays(-(int)currentDate.DayOfWeek));

        // Get start and end of month
        var (startMonth, endMonth) = DateHelper.GetStartEndOfDay(new DateTime(currentDate.Year, currentDate.Month, 1));

        // Get start and end of year
        var (startYear, endYear) = DateHelper.GetStartEndOfDay(new DateTime(currentDate.Year, 1, 1));

        var totalNumberOfStudents = 10;
        var totalNumberOfStudySessions = await _adminService.GetTotalNumberOfStudySessionsAsync(startMonth, endMonth);
        var totalNumberOfBreakTimeMinutes = await _adminService.GetTotalNumberOfBreaksAsync(startMonth, endMonth);
        var totalStudyTimeLogged = await _adminService.GetTotalStudyTimeLoggedAsync(startMonth, endMonth);
        var totalBreakTimeLogged = await _adminService.GetTotalBreakTimeLoggedAsync(startMonth, endMonth);

        Console.WriteLine(startMonth);
        Console.WriteLine(endMonth);

        var studySessionsByMonth = await _adminService.GetStudySessionsByRangeAsync(startMonth, endMonth);
        var breaksByMonth = await _adminService.GetBreaksByRangeAsync(startMonth, endMonth);
        var studySessionsByYear = await _adminService.GetStudySessionsByRangeAsync(startYear, endYear);
        var breaksByYear = await _adminService.GetBreaksByRangeAsync(startYear, endYear);

        var studySessionsByWeek = await _adminService.GetStudySessionsByRangeAsync(startOfWeek, endOfWeek);

        var metrics = await _adminService.GetStudentMetricsAsync(
            totalNumberOfStudents,
            totalNumberOfStudySessions,
            totalNumberOfBreakTimeMinutes,
            totalStudyTimeLogged,
            totalBreakTimeLogged,
            studySessionsByMonth,
            breaksByMonth,
            studySessionsByYear,
            breaksByYear
        );

        _logger.LogInformation("Admin metrics created successfully");

        return Ok(metrics);
    }
}