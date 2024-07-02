using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityStudentTracker.API.Models.Domains;
using UniversityStudentTracker.API.Models.DTO.StudySession;
using UniversityStudentTracker.API.Repositories;
using UniversityStudentTracker.API.Utils;

namespace UniversityStudentTracker.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class StudySessionsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IStudySessionRepository _studySessionRepository;

    public StudySessionsController(IMapper mapper, IStudySessionRepository studySessionRepository)
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
    [ValidateModel]
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