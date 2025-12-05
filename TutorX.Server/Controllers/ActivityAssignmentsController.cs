using Microsoft.AspNetCore.Mvc;
using TutorX.Infrastructure.Entities;
using TutorX.Infrastructure.Repositories;

namespace TutorX.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ActivityAssignmentsController : ControllerBase
{
    private readonly IActivityAssignmentRepository _assignmentRepository;

    public ActivityAssignmentsController(IActivityAssignmentRepository assignmentRepository)
    {
        _assignmentRepository = assignmentRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ActivityAssignment>>> GetActivityAssignments()
    {
        var assignments = await _assignmentRepository.GetAssignmentsWithDetailsAsync();
        return Ok(assignments);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ActivityAssignment>> GetActivityAssignment(int id)
    {
        var assignment = await _assignmentRepository.GetByIdAsync(id);

        if (assignment == null)
        {
            return NotFound();
        }

        return Ok(assignment);
    }

    [HttpPost]
    public async Task<ActionResult<ActivityAssignment>> CreateActivityAssignment(ActivityAssignment assignment)
    {
        await _assignmentRepository.AddAsync(assignment);
        await _assignmentRepository.SaveChangesAsync();

        return CreatedAtAction(nameof(GetActivityAssignment), new { id = assignment.Id }, assignment);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateActivityAssignment(int id, ActivityAssignment assignment)
    {
        if (id != assignment.Id)
        {
            return BadRequest();
        }

        var existingAssignment = await _assignmentRepository.GetByIdAsync(id);
        if (existingAssignment == null)
        {
            return NotFound();
        }

        _assignmentRepository.Update(assignment);
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