using Microsoft.AspNetCore.Mvc;
using TutorX.Infrastructure.Entities;
using TutorX.Infrastructure.Repositories;

namespace TutorX.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentGroupsController : ControllerBase
{
 private readonly IStudentGroupRepository _groupRepository;

 public StudentGroupsController(IStudentGroupRepository groupRepository)
 {
 _groupRepository = groupRepository;
 }

 [HttpGet]
 public async Task<ActionResult<IEnumerable<StudentGroup>>> GetStudentGroups()
 {
 var groups = await _groupRepository.GetGroupsWithStudentsAsync();
 return Ok(groups);
 }

 [HttpGet("{id}")]
 public async Task<ActionResult<StudentGroup>> GetStudentGroup(int id)
 {
 var group = await _groupRepository.GetByIdAsync(id);

 if (group == null)
 {
 return NotFound();
 }

 return Ok(group);
 }

 [HttpPost]
 public async Task<ActionResult<StudentGroup>> CreateStudentGroup(StudentGroup group)
 {
 await _groupRepository.AddAsync(group);
 await _groupRepository.SaveChangesAsync();

 return CreatedAtAction(nameof(GetStudentGroup), new { id = group.Id }, group);
 }

 [HttpPut("{id}")]
 public async Task<IActionResult> UpdateStudentGroup(int id, StudentGroup group)
 {
 if (id != group.Id)
 {
 return BadRequest();
 }

 var existingGroup = await _groupRepository.GetByIdAsync(id);
 if (existingGroup == null)
 {
 return NotFound();
 }

 _groupRepository.Update(group);
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