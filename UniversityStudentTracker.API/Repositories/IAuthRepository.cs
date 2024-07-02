using Microsoft.AspNetCore.Identity;

namespace UniversityStudentTracker.API.Repositories;

public interface IAuthRepository
{
    string CreateJwtToken(IdentityUser user, List<string> roles);
}