using System.ComponentModel.DataAnnotations;
using UniversityStudentTracker.API.Utils;

namespace UniversityStudentTracker.API.Models.DTO.Auth;

[RegisterRequestValidations]
public class RegisterRequestDto
{
    [Required(ErrorMessage = "Username is required.")]
    [StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    public string Username { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address format.")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}