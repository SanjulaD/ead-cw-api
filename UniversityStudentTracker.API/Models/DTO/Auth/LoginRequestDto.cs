using System.ComponentModel.DataAnnotations;

namespace UniversityStudentTracker.API.Models.DTO.Auth;

public class LoginRequestDto
{
    [Required(ErrorMessage = "Username is required")]
    [DataType(DataType.EmailAddress, ErrorMessage = "Invalid email format")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}