using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityStudentTracker.API.Models.Domains;
using UniversityStudentTracker.API.Models.DTO.Break;
using UniversityStudentTracker.API.Services;
using UniversityStudentTracker.API.Utils;

namespace UniversityStudentTracker.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BreaksController : ControllerBase
{
    private readonly BreakService _breakService;
    private readonly ILogger<BreaksController> _logger;
    private readonly IMapper _mapper;

    public BreaksController(BreakService breakService, IMapper mapper, ILogger<BreaksController> logger)
    {
        _breakService = breakService;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var breaksDomainModel = await _breakService.GetAllAsync();
        var breaksDto = _mapper.Map<List<BreakDto>>(breaksDomainModel);

        _logger.LogInformation("Retrieved {BreakCount} breaks.", breaksDto.Count);

        return Ok(breaksDto);
    }

    [HttpPost]
    [ValidateModel]
    public async Task<IActionResult> Create([FromBody] AddBreakDto addBreakDto)
    {
        var breaksDomainModel = _mapper.Map<Break>(addBreakDto);
        await _breakService.CreateAsync(breaksDomainModel);

        var breaksDto = _mapper.Map<BreakDto>(breaksDomainModel);

        _logger.LogInformation("Break created with ID {BreakId}.", breaksDto.BreakID);

        return Ok(breaksDto);
    }

    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var breaksDomainModel = await _breakService.GetByIdAsync(id);
        if (breaksDomainModel == null)
        {
            _logger.LogWarning("Break not found with ID {BreakId}.", id);
            return NotFound();
        }

        var breaksDto = _mapper.Map<BreakDto>(breaksDomainModel);

        _logger.LogInformation("Retrieved break with ID {BreakId}.", id);

        return Ok(breaksDto);
    }

    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task<IActionResult> DeleteById([FromRoute] Guid id)
    {
        var breaksDomainModel = await _breakService.DeleteAsync(id);
        if (breaksDomainModel == null)
        {
            _logger.LogWarning("Break not found with ID {BreakId} for deletion.", id);
            return NotFound();
        }

        var breaksDto = _mapper.Map<BreakDto>(breaksDomainModel);

        _logger.LogInformation("Break deleted with ID {BreakId}.", id);

        return Ok(breaksDto);
    }
}