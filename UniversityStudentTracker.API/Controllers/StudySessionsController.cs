using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityStudentTracker.API.Models.Domains;
using UniversityStudentTracker.API.Models.DTO.StudySession;
using UniversityStudentTracker.API.Services;
using UniversityStudentTracker.API.Utils;

namespace UniversityStudentTracker.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class StudySessionsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly StudySessionService _studySessionService;

    public StudySessionsController(IMapper mapper, StudySessionService studySessionService)
    {
        _mapper = mapper;
        _studySessionService = studySessionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var studySessionDomainModel = await _studySessionService.GetAllAsync();
        var studySessionDto = _mapper.Map<List<StudySessionDto>>(studySessionDomainModel);

        return Ok(studySessionDto);
    }

    [HttpPost]
    [ValidateModel]
    public async Task<IActionResult> Create([FromBody] AddStudySessionDto addStudySessionDto)
    {
        var studySessionDomainModel = _mapper.Map<StudySession>(addStudySessionDto);
        await _studySessionService.CreateAsync(studySessionDomainModel);

        var studySessionDto = _mapper.Map<StudySessionDto>(studySessionDomainModel);

        return Ok(studySessionDto);
    }

    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var studySessionDomainModel = await _studySessionService.GetByIdAsync(id);
        if (studySessionDomainModel == null) return NotFound();

        var studySessionDto = _mapper.Map<StudySessionDto>(studySessionDomainModel);
        return Ok(studySessionDto);
    }

    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task<IActionResult> DeleteById([FromRoute] Guid id)
    {
        var studySessionDomainModel = await _studySessionService.DeleteAsync(id);
        var studySessionDto = _mapper.Map<StudySessionDto>(studySessionDomainModel);
        return Ok(studySessionDto);
    }
}