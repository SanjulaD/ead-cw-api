using Microsoft.AspNetCore.Identity;
using UniversityStudentTracker.API.Models.Domains;

namespace UniversityStudentTracker.API.Models.Navigation;

public class ApplicationUser : IdentityUser<Guid>
{
    public ICollection<StudySession> StudySessions { get; set; }
    public ICollection<Break> Breaks { get; set; }
    public ICollection<Prediction> Predictions { get; set; }
}