using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using UniversityStudentTracker.API.Models.DTO.Auth;

namespace UniversityStudentTracker.API.Utils;

public class RegisterRequestValidations : ValidationAttribute
{
    private readonly string[] _validRoles = ["Student", "Admin"];

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not RegisterRequestDto dto) return ValidationResult.Success;
        if (!Regex.IsMatch(dto.Password, @"[A-Z]"))
            return new ValidationResult("Password must contain at least one uppercase letter.");

        return ValidationResult.Success;
    }
}