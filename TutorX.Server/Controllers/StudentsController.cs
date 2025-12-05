using Microsoft.AspNetCore.Mvc;
using TutorX.Infrastructure.Entities;
using TutorX.Infrastructure.Repositories;

namespace TutorX.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
 private readonly IStudentRepository _studentRepository;

 public StudentsController(IStudentRepository studentRepository)
 {
 _studentRepository = studentRepository;
 }

 [HttpGet]
 public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
 {
 var students = await _studentRepository.GetAllAsync();
 return Ok(students);
 }

 [HttpGet("{id}")]
 public async Task<ActionResult<Student>> GetStudent(int id)
 {
 var student = await _studentRepository.GetByIdAsync(id);

 if (student == null)
 {
 return NotFound();
 }

 return Ok(student);
 }

 [HttpPost]
 public async Task<ActionResult<Student>> CreateStudent(Student student)
 {
 await _studentRepository.AddAsync(student);
 await _studentRepository.SaveChangesAsync();

 return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, student);
 }

 [HttpPut("{id}")]
 public async Task<IActionResult> UpdateStudent(int id, Student student)
 {
 if (id != student.Id)
 {
 return BadRequest();
 }

 var existingStudent = await _studentRepository.GetByIdAsync(id);
 if (existingStudent == null)
 {
 return NotFound();
 }

 _studentRepository.Update(student);
 await _studentRepository.SaveChangesAsync();

 return NoContent();
 }

 [HttpDelete("{id}")]
 public async Task<IActionResult> DeleteStudent(int id)
 {
 var student = await _studentRepository.GetByIdAsync(id);
 if (student == null)
 {
 return NotFound();
 }

 _studentRepository.Remove(student);
 await _studentRepository.SaveChangesAsync();

 return NoContent();
 }
}