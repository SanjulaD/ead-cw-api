using UniversityStudentTracker.API.Models.Domains;
using UniversityStudentTracker.API.Repositories;

namespace UniversityStudentTracker.API.Services;

public class BreakService : IBreakInterface
{
    private readonly IBreakInterface _breakRepository;

    public BreakService(IBreakInterface breakRepository)
    {
        _breakRepository = breakRepository;
    }

    public async Task<List<Break>> GetAllAsync()
    {
        return await _breakRepository.GetAllAsync();
    }

    public async Task<Break> CreateAsync(Break studyBreak)
    {
        return await _breakRepository.CreateAsync(studyBreak);
    }

    public async Task<Break?> GetByIdAsync(Guid id)
    {
        return await _breakRepository.GetByIdAsync(id);
    }

    public async Task<Break?> DeleteAsync(Guid id)
    {
        return await _breakRepository.DeleteAsync(id);
    }
}