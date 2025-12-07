using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TutorX.Infrastructure;
using TutorX.Infrastructure.Entities;
using TutorX.Infrastructure.Repositories;
using TutorX.Server.Services;
using TutorX.Shared.DTOs;

namespace TutorX.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DrawsController : ControllerBase
{
    private readonly IDrawRepository _drawRepository;
    private readonly IMappingService _mappingService;
    private readonly TutorXDbContext _context;

    public DrawsController(IDrawRepository drawRepository, IMappingService mappingService, TutorXDbContext context)
    {
        _drawRepository = drawRepository;
        _mappingService = mappingService;
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DrawDto>>> GetDraws()
    {
        var draws = await _drawRepository.GetDrawsWithDetailsAsync();
        var drawDtos = draws.Select(_mappingService.MapToDto);
        return Ok(drawDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DrawDto>> GetDraw(int id)
    {
        var draw = await _drawRepository.GetDrawWithResultsAsync(id);

        if (draw == null)
        {
            return NotFound();
        }

        return Ok(_mappingService.MapToDto(draw));
    }

    [HttpPost]
    public async Task<ActionResult<DrawDto>> CreateDraw(CreateDrawDto createDto)
    {
        var draw = _mappingService.MapToEntity(createDto);
        await _drawRepository.AddAsync(draw);
        await _drawRepository.SaveChangesAsync();

        // Perform random selection if students are provided
        if (createDto.StudentIds.Any())
        {
            var random = new Random();
            var numberToSelect = Math.Min(createDto.NumberToSelect, createDto.StudentIds.Count);
            var selectedStudentIds = createDto.StudentIds.OrderBy(x => random.Next()).Take(numberToSelect).ToList();

            foreach (var studentId in selectedStudentIds)
            {
                var drawResult = new DrawResult
                {
                    DrawId = draw.Id,
                    StudentId = studentId
                };
                _context.DrawResults.Add(drawResult);
            }

            await _context.SaveChangesAsync();
        }

        // Reload draw with results
        var createdDraw = await _drawRepository.GetDrawWithResultsAsync(draw.Id);
        var drawDto = _mappingService.MapToDto(createdDraw!);
        return CreatedAtAction(nameof(GetDraw), new { id = draw.Id }, drawDto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDraw(int id)
    {
        var draw = await _drawRepository.GetByIdAsync(id);
        if (draw == null)
        {
            return NotFound();
        }

        _drawRepository.Remove(draw);
        await _drawRepository.SaveChangesAsync();

        return NoContent();
    }
}