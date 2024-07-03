using System.Security.Claims;
using UniversityStudentTracker.API.Repositories;

namespace UniversityStudentTracker.API.Utils;

public class UserService(IHttpContextAccessor httpContextAccessor) : IUserInterface
{
    public Guid GetUserId()
    {
        var userIdString = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userIdString == null)
        {
            throw new InvalidOperationException("User ID claim not found.");
        }
        else
        {
            if (!Guid.TryParse(userIdString, out var userIdGuid))
                throw new FormatException($"Failed to parse user ID '{userIdString}' to Guid.");

            return userIdGuid;
        }
    }
}