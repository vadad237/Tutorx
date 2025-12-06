using Microsoft.AspNetCore.Mvc;
using TutorX.Infrastructure.Repositories;
using TutorX.Server.Services;
using TutorX.Shared.DTOs;

namespace TutorX.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentRepository _studentRepository;
    private readonly IMappingService _mappingService;

    public StudentsController(IStudentRepository studentRepository, IMappingService mappingService)
    {
        _studentRepository = studentRepository;
        _mappingService = mappingService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudents()
    {
        var students = await _studentRepository.GetAllAsync();
        var studentDtos = students.Select(_mappingService.MapToDto);
        return Ok(studentDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<StudentDto>> GetStudent(int id)
    {
        var student = await _studentRepository.GetByIdAsync(id);

        if (student == null)
        {
            return NotFound();
        }

        return Ok(_mappingService.MapToDto(student));
    }

    [HttpPost]
    public async Task<ActionResult<StudentDto>> CreateStudent(CreateStudentDto createDto)
    {
        var student = _mappingService.MapToEntity(createDto);
        await _studentRepository.AddAsync(student);
        await _studentRepository.SaveChangesAsync();

        var studentDto = _mappingService.MapToDto(student);
        return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, studentDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStudent(int id, UpdateStudentDto updateDto)
    {
        var existingStudent = await _studentRepository.GetByIdAsync(id);
        if (existingStudent == null)
        {
            return NotFound();
        }

        _mappingService.MapToEntity(updateDto, existingStudent);
        _studentRepository.Update(existingStudent);
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