using Microsoft.AspNetCore.Mvc;
using TutorX.Infrastructure.Repositories;
using TutorX.Server.Services;
using TutorX.Shared.DTOs;

namespace TutorX.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ActivitiesController : ControllerBase
{
    private readonly IActivityRepository _activityRepository;
    private readonly IMappingService _mappingService;

    public ActivitiesController(IActivityRepository activityRepository, IMappingService mappingService)
    {
        _activityRepository = activityRepository;
        _mappingService = mappingService;
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
        var activity = await _activityRepository.GetByIdAsync(id);

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
        await _activityRepository.AddAsync(activity);
        await _activityRepository.SaveChangesAsync();

        var activityDto = _mappingService.MapToDto(activity);
        return CreatedAtAction(nameof(GetActivity), new { id = activity.Id }, activityDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateActivity(int id, UpdateActivityDto updateDto)
    {
        var existingActivity = await _activityRepository.GetByIdAsync(id);
        if (existingActivity == null)
        {
            return NotFound();
        }

        _mappingService.MapToEntity(updateDto, existingActivity);
        _activityRepository.Update(existingActivity);
        await _activityRepository.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteActivity(int id)
    {
        var activity = await _activityRepository.GetByIdAsync(id);
        if (activity == null)
        {
            return NotFound();
        }

        _activityRepository.Remove(activity);
        await _activityRepository.SaveChangesAsync();

        return NoContent();
    }
}