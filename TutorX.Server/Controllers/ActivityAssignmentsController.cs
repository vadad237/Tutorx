using Microsoft.AspNetCore.Mvc;
using TutorX.Infrastructure.Repositories;
using TutorX.Server.Services;
using TutorX.Shared.DTOs;

namespace TutorX.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ActivityAssignmentsController : ControllerBase
{
    private readonly IActivityAssignmentRepository _assignmentRepository;
    private readonly IMappingService _mappingService;

    public ActivityAssignmentsController(IActivityAssignmentRepository assignmentRepository, IMappingService mappingService)
    {
        _assignmentRepository = assignmentRepository;
        _mappingService = mappingService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ActivityAssignmentDto>>> GetActivityAssignments()
    {
        var assignments = await _assignmentRepository.GetAssignmentsWithDetailsAsync();
        var assignmentDtos = assignments.Select(_mappingService.MapToDto);
        return Ok(assignmentDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ActivityAssignmentDto>> GetActivityAssignment(int id)
    {
        var assignment = await _assignmentRepository.GetByIdAsync(id);

        if (assignment == null)
        {
            return NotFound();
        }

        return Ok(_mappingService.MapToDto(assignment));
    }

    [HttpPost]
    public async Task<ActionResult<ActivityAssignmentDto>> CreateActivityAssignment(CreateActivityAssignmentDto createDto)
    {
        var assignment = _mappingService.MapToEntity(createDto);
        await _assignmentRepository.AddAsync(assignment);
        await _assignmentRepository.SaveChangesAsync();

        var assignmentDto = _mappingService.MapToDto(assignment);
        return CreatedAtAction(nameof(GetActivityAssignment), new { id = assignment.Id }, assignmentDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateActivityAssignment(int id, UpdateActivityAssignmentDto updateDto)
    {
        var existingAssignment = await _assignmentRepository.GetByIdAsync(id);
        if (existingAssignment == null)
        {
            return NotFound();
        }

        _mappingService.MapToEntity(updateDto, existingAssignment);
        _assignmentRepository.Update(existingAssignment);
        await _assignmentRepository.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteActivityAssignment(int id)
    {
        var assignment = await _assignmentRepository.GetByIdAsync(id);
        if (assignment == null)
        {
            return NotFound();
        }

        _assignmentRepository.Remove(assignment);
        await _assignmentRepository.SaveChangesAsync();

        return NoContent();
    }
}