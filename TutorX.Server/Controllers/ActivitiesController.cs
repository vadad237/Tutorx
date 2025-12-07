using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TutorX.Infrastructure;
using TutorX.Infrastructure.Repositories;
using TutorX.Server.Services;
using TutorX.Shared.DTOs;

namespace TutorX.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ActivitiesController : ControllerBase
{
    private readonly IActivityRepository _activityRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly IMappingService _mappingService;
    private readonly TutorXDbContext _context;

    public ActivitiesController(
        IActivityRepository activityRepository,
        IStudentRepository studentRepository,
        IMappingService mappingService,
        TutorXDbContext context)
    {
        _activityRepository = activityRepository;
        _studentRepository = studentRepository;
        _mappingService = mappingService;
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ActivityDto>>> GetActivities()
    {
        var activities = await _activityRepository.GetActivitiesWithGroupsAsync();
        var activityDtos = activities.Select(_mappingService.MapToDto);
        return Ok(activityDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ActivityDto>> GetActivity(int id)
    {
        var activity = await _context.Activities.Include(a => a.Group).Include(a => a.Students).FirstOrDefaultAsync(a => a.Id == id);

        if (activity == null)
        {
            return NotFound();
        }

        return Ok(_mappingService.MapToDto(activity));
    }

    [HttpPost]
    public async Task<ActionResult<ActivityDto>> CreateActivity(CreateActivityDto createDto)
    {
        var activity = _mappingService.MapToEntity(createDto);

        // Load students if specified
        if (createDto.StudentIds.Any())
        {
            var students = await _context.Students.Where(s => createDto.StudentIds.Contains(s.Id)).ToListAsync();
            activity.Students = students;
        }

        await _activityRepository.AddAsync(activity);
        await _activityRepository.SaveChangesAsync();

        // Reload activity with students
        var createdActivity = await _context.Activities.Include(a => a.Students).FirstOrDefaultAsync(a => a.Id == activity.Id);
        var activityDto = _mappingService.MapToDto(createdActivity!);
        return CreatedAtAction(nameof(GetActivity), new { id = activity.Id }, activityDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateActivity(int id, UpdateActivityDto updateDto)
    {
        var existingActivity = await _context.Activities.Include(a => a.Students).FirstOrDefaultAsync(a => a.Id == id);
        if (existingActivity == null)
        {
            return NotFound();
        }

        _mappingService.MapToEntity(updateDto, existingActivity);

        // Update students
        existingActivity.Students.Clear();
        if (updateDto.StudentIds.Any())
        {
            var students = await _context.Students.Where(s => updateDto.StudentIds.Contains(s.Id)).ToListAsync();
            foreach (var student in students)
            {
                existingActivity.Students.Add(student);
            }
        }

        _activityRepository.Update(existingActivity);
        await _activityRepository.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteActivity(int id)
    {
        // Load activity with all related entities
        var activity = await _context.Activities
            .Include(a => a.Students)
            .Include(a => a.Assignments)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (activity == null)
        {
            return NotFound();
        }

        // Delete all associated draws and their results
        var associatedDraws = await _context.Draws
            .Include(d => d.Results)
            .Where(d => d.ActivityId == id)
            .ToListAsync();

        if (associatedDraws.Any())
        {
            // Remove all draw results first
            foreach (var draw in associatedDraws)
            {
                _context.DrawResults.RemoveRange(draw.Results);
            }

            // Remove all draws
            _context.Draws.RemoveRange(associatedDraws);
        }

        // Clear the many-to-many relationship with students
        activity.Students.Clear();

        // Remove activity assignments if any
        if (activity.Assignments.Any())
        {
            _context.ActivityAssignments.RemoveRange(activity.Assignments);
        }

        // Now remove the activity
        _activityRepository.Remove(activity);
        await _activityRepository.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost("{activityId}/students/{studentId}")]
    public async Task<IActionResult> AddStudentToActivity(int activityId, int studentId)
    {
        var activity = await _context.Activities.Include(a => a.Students).FirstOrDefaultAsync(a => a.Id == activityId);
        if (activity == null)
        {
            return NotFound("Activity not found");
        }

        var student = await _studentRepository.GetByIdAsync(studentId);
        if (student == null)
        {
            return NotFound("Student not found");
        }

        if (!activity.Students.Any(s => s.Id == studentId))
        {
            activity.Students.Add(student);
            await _context.SaveChangesAsync();
        }

        return Ok();
    }

    [HttpPost("{activityId}/students/bulk")]
    public async Task<IActionResult> AddStudentsToActivity(int activityId, [FromBody] List<int> studentIds)
    {
        var activity = await _context.Activities.Include(a => a.Students).FirstOrDefaultAsync(a => a.Id == activityId);
        if (activity == null)
        {
            return NotFound("Activity not found");
        }

        var students = await _context.Students.Where(s => studentIds.Contains(s.Id)).ToListAsync();

        foreach (var student in students)
        {
            if (!activity.Students.Any(s => s.Id == student.Id))
            {
                activity.Students.Add(student);
            }
        }

        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("{activityId}/students/{studentId}")]
    public async Task<IActionResult> RemoveStudentFromActivity(int activityId, int studentId)
    {
        var activity = await _context.Activities.Include(a => a.Students).FirstOrDefaultAsync(a => a.Id == activityId);
        if (activity == null)
        {
            return NotFound("Activity not found");
        }

        var student = activity.Students.FirstOrDefault(s => s.Id == studentId);
        if (student != null)
        {
            activity.Students.Remove(student);
            await _context.SaveChangesAsync();
        }

        return Ok();
    }
}