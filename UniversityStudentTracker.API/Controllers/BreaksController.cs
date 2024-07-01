using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UniversityStudentTracker.API.Models.Domains;
using UniversityStudentTracker.API.Models.DTO.Break;
using UniversityStudentTracker.API.Repositories;

namespace UniversityStudentTracker.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BreaksController : ControllerBase
{
    private readonly IBreaksRepository _breaksRepository;
    private readonly IMapper _mapper;

    public BreaksController(IBreaksRepository breaksRepository, IMapper mapper)
    {
        _breaksRepository = breaksRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var breaksDomainModel = await _breaksRepository.GetAllAsync();
        var breaksDto = _mapper.Map<List<BreakDto>>(breaksDomainModel);

        return Ok(breaksDto);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AddBreakDto addBreakDto)
    {
        var breaksDomainModel = _mapper.Map<Break>(addBreakDto);
        await _breaksRepository.CreateAsync(breaksDomainModel);

        var breaksDto = _mapper.Map<BreakDto>(breaksDomainModel);

        return Ok(breaksDto);
    }

    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var breaksDomainModel = await _breaksRepository.GetByIdAsync(id);
        if (breaksDomainModel == null) return NotFound();

        var breaksDto = _mapper.Map<Break>(breaksDomainModel);
        return Ok(breaksDto);
    }

    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task<IActionResult> DeleteById([FromRoute] Guid id)
    {
        var breaksDomainModel = await _breaksRepository.DeleteAsync(id);
        var breaksDto = _mapper.Map<BreakDto>(breaksDomainModel);
        return Ok(breaksDto);
    }
}