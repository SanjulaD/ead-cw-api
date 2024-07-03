using Microsoft.AspNetCore.Identity;

namespace UniversityStudentTracker.API.Repositories;

public interface IAuthInterface
{
    string CreateJwtToken(IdentityUser user, List<string> roles);
}