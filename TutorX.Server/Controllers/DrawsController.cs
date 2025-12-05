using Microsoft.AspNetCore.Mvc;
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

 public DrawsController(IDrawRepository drawRepository, IMappingService mappingService)
 {
 _drawRepository = drawRepository;
 _mappingService = mappingService;
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

 var drawDto = _mappingService.MapToDto(draw);
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