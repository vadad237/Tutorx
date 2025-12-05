using Microsoft.AspNetCore.Mvc;
using TutorX.Infrastructure.Repositories;
using TutorX.Server.Services;
using TutorX.Shared.DTOs;

namespace TutorX.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentGroupsController : ControllerBase
{
    private readonly IStudentGroupRepository _groupRepository;
    private readonly IMappingService _mappingService;

    public StudentGroupsController(IStudentGroupRepository groupRepository, IMappingService mappingService)
    {
        _groupRepository = groupRepository;
        _mappingService = mappingService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StudentGroupDto>>> GetStudentGroups()
    {
        var groups = await _groupRepository.GetGroupsWithStudentsAsync();
        var groupDtos = groups.Select(_mappingService.MapToDto);
        return Ok(groupDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<StudentGroupDto>> GetStudentGroup(int id)
    {
        var group = await _groupRepository.GetByIdAsync(id);

        if (group == null)
        {
            return NotFound();
        }

        return Ok(_mappingService.MapToDto(group));
    }

    [HttpPost]
    public async Task<ActionResult<StudentGroupDto>> CreateStudentGroup(CreateStudentGroupDto createDto)
    {
        var group = _mappingService.MapToEntity(createDto);
        await _groupRepository.AddAsync(group);
        await _groupRepository.SaveChangesAsync();

        var groupDto = _mappingService.MapToDto(group);
        return CreatedAtAction(nameof(GetStudentGroup), new { id = group.Id }, groupDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStudentGroup(int id, UpdateStudentGroupDto updateDto)
    {
        var existingGroup = await _groupRepository.GetByIdAsync(id);
        if (existingGroup == null)
        {
            return NotFound();
        }

        _mappingService.MapToEntity(updateDto, existingGroup);
        _groupRepository.Update(existingGroup);
        await _groupRepository.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudentGroup(int id)
    {
        var group = await _groupRepository.GetByIdAsync(id);
        if (group == null)
        {
            return NotFound();
        }

        _groupRepository.Remove(group);
        await _groupRepository.SaveChangesAsync();

        return NoContent();
    }
}