using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UniversityStudentTracker.API.Models.Domains;
using UniversityStudentTracker.API.Models.DTO.StudySession;
using UniversityStudentTracker.API.Repositories;

namespace UniversityStudentTracker.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudySessionController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IStudySessionRepository _studySessionRepository;

    public StudySessionController(IMapper mapper, IStudySessionRepository studySessionRepository)
    {
        _mapper = mapper;
        _studySessionRepository = studySessionRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var studySessionDomainModel = await _studySessionRepository.GetAllAsync();
        var studySessionDto = _mapper.Map<List<StudySessionDto>>(studySessionDomainModel);

        return Ok(studySessionDto);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddStudySessionDto addStudySessionDto)
    {
        var studySessionDomainModel = _mapper.Map<StudySession>(addStudySessionDto);
        await _studySessionRepository.CreateAsync(studySessionDomainModel);

        var studySessionDto = _mapper.Map<StudySessionDto>(studySessionDomainModel);

        return Ok(studySessionDto);
    }

    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var studySessionDomainModel = await _studySessionRepository.GetByIdAsync(id);
        if (studySessionDomainModel == null) return NotFound();

        var studySessionDto = _mapper.Map<StudySessionDto>(studySessionDomainModel);
        return Ok(studySessionDto);
    }

    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task<IActionResult> DeleteById([FromRoute] Guid id)
    {
        var studySessionDomainModel = await _studySessionRepository.DeleteAsync(id);
        var studySessionDto = _mapper.Map<StudySessionDto>(studySessionDomainModel);
        return Ok(studySessionDto);
    }
}