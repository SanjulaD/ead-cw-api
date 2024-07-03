using UniversityStudentTracker.API.Models.Domains;

namespace UniversityStudentTracker.API.Repositories;

public interface IBreakInterface
{
    Task<List<Break>> GetAllAsync();
    Task<Break> CreateAsync(Break studyBreak);
    Task<Break?> GetByIdAsync(Guid id);
    Task<Break?> DeleteAsync(Guid id);
}