using Microsoft.AspNetCore.Mvc;
using TutorX.Infrastructure.Entities;
using TutorX.Infrastructure.Repositories;

namespace TutorX.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DrawsController : ControllerBase
{
 private readonly IDrawRepository _drawRepository;

 public DrawsController(IDrawRepository drawRepository)
 {
 _drawRepository = drawRepository;
 }

 [HttpGet]
 public async Task<ActionResult<IEnumerable<Draw>>> GetDraws()
 {
 var draws = await _drawRepository.GetDrawsWithDetailsAsync();
 return Ok(draws);
 }

 [HttpGet("{id}")]
 public async Task<ActionResult<Draw>> GetDraw(int id)
 {
 var draw = await _drawRepository.GetDrawWithResultsAsync(id);

 if (draw == null)
 {
 return NotFound();
 }

 return Ok(draw);
 }

 [HttpPost]
 public async Task<ActionResult<Draw>> CreateDraw(Draw draw)
 {
 await _drawRepository.AddAsync(draw);
 await _drawRepository.SaveChangesAsync();

 return CreatedAtAction(nameof(GetDraw), new { id = draw.Id }, draw);
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