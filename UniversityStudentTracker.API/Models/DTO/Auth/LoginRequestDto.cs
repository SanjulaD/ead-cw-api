using System.ComponentModel.DataAnnotations;

namespace UniversityStudentTracker.API.Models.DTO.Auth;

public class LoginRequestDto
{
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Username { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}