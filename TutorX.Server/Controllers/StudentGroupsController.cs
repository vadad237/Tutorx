using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TutorX.Infrastructure;
using TutorX.Infrastructure.Repositories;
using TutorX.Server.Services;
using TutorX.Shared.DTOs;

namespace TutorX.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentGroupsController : ControllerBase
{
    private readonly IStudentGroupRepository _groupRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly IMappingService _mappingService;
    private readonly TutorXDbContext _context;

    public StudentGroupsController(
        IStudentGroupRepository groupRepository, 
        IStudentRepository studentRepository,
        IMappingService mappingService,
        TutorXDbContext context)
    {
        _groupRepository = groupRepository;
        _studentRepository = studentRepository;
        _mappingService = mappingService;
        _context = context;
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
        var group = await _context.StudentGroups.Include(g => g.Students).FirstOrDefaultAsync(g => g.Id == id);

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

    [HttpPost("{groupId}/students/{studentId}")]
    public async Task<IActionResult> AddStudentToGroup(int groupId, int studentId)
    {
        var group = await _context.StudentGroups.Include(g => g.Students).FirstOrDefaultAsync(g => g.Id == groupId);
        if (group == null)
        {
            return NotFound("Group not found");
        }

        var student = await _studentRepository.GetByIdAsync(studentId);
        if (student == null)
        {
            return NotFound("Student not found");
        }

        if (!group.Students.Any(s => s.Id == studentId))
        {
            group.Students.Add(student);
            await _context.SaveChangesAsync();
        }

        return Ok();
    }

    [HttpDelete("{groupId}/students/{studentId}")]
    public async Task<IActionResult> RemoveStudentFromGroup(int groupId, int studentId)
    {
        var group = await _context.StudentGroups.Include(g => g.Students).FirstOrDefaultAsync(g => g.Id == groupId);
        if (group == null)
        {
            return NotFound("Group not found");
        }

        var student = group.Students.FirstOrDefault(s => s.Id == studentId);
        if (student != null)
        {
            group.Students.Remove(student);
            await _context.SaveChangesAsync();
        }

        return Ok();
    }
}