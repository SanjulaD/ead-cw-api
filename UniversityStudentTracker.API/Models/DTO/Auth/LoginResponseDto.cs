namespace UniversityStudentTracker.API.Models.DTO.Auth;

public class LoginResponseDto
{
    public string JwtToken { get; set; }
    public string UserId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
}