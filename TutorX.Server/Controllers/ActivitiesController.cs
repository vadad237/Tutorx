using Microsoft.AspNetCore.Mvc;
using TutorX.Infrastructure.Entities;
using TutorX.Infrastructure.Repositories;

namespace TutorX.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ActivitiesController : ControllerBase
{
 private readonly IActivityRepository _activityRepository;

 public ActivitiesController(IActivityRepository activityRepository)
 {
 _activityRepository = activityRepository;
 }

 [HttpGet]
 public async Task<ActionResult<IEnumerable<Activity>>> GetActivities()
 {
 var activities = await _activityRepository.GetActivitiesWithGroupsAsync();
 return Ok(activities);
 }

 [HttpGet("{id}")]
 public async Task<ActionResult<Activity>> GetActivity(int id)
 {
 var activity = await _activityRepository.GetByIdAsync(id);

 if (activity == null)
 {
 return NotFound();
 }

 return Ok(activity);
 }

 [HttpPost]
 public async Task<ActionResult<Activity>> CreateActivity(Activity activity)
 {
 await _activityRepository.AddAsync(activity);
 await _activityRepository.SaveChangesAsync();

 return CreatedAtAction(nameof(GetActivity), new { id = activity.Id }, activity);
 }

 [HttpPut("{id}")]
 public async Task<IActionResult> UpdateActivity(int id, Activity activity)
 {
 if (id != activity.Id)
 {
 return BadRequest();
 }

 var existingActivity = await _activityRepository.GetByIdAsync(id);
 if (existingActivity == null)
 {
 return NotFound();
 }

 _activityRepository.Update(activity);
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