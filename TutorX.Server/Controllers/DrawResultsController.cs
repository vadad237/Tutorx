using Microsoft.AspNetCore.Mvc;
using TutorX.Infrastructure.Entities;
using TutorX.Infrastructure.Repositories;

namespace TutorX.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DrawResultsController : ControllerBase
{
 private readonly IDrawResultRepository _drawResultRepository;

 public DrawResultsController(IDrawResultRepository drawResultRepository)
 {
 _drawResultRepository = drawResultRepository;
 }

 [HttpGet]
 public async Task<ActionResult<IEnumerable<DrawResult>>> GetDrawResults()
 {
 var results = await _drawResultRepository.GetResultsWithDetailsAsync();
 return Ok(results);
 }

 [HttpGet("{id}")]
 public async Task<ActionResult<DrawResult>> GetDrawResult(int id)
 {
 var result = await _drawResultRepository.GetByIdAsync(id);

 if (result == null)
 {
 return NotFound();
 }

 return Ok(result);
 }
}