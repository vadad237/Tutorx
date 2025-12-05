using Microsoft.AspNetCore.Mvc;
using TutorX.Infrastructure.Repositories;
using TutorX.Server.Services;
using TutorX.Shared.DTOs;

namespace TutorX.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DrawResultsController : ControllerBase
{
 private readonly IDrawResultRepository _drawResultRepository;
 private readonly IMappingService _mappingService;

 public DrawResultsController(IDrawResultRepository drawResultRepository, IMappingService mappingService)
 {
 _drawResultRepository = drawResultRepository;
 _mappingService = mappingService;
 }

 [HttpGet]
 public async Task<ActionResult<IEnumerable<DrawResultDto>>> GetDrawResults()
 {
 var results = await _drawResultRepository.GetResultsWithDetailsAsync();
 var resultDtos = results.Select(_mappingService.MapToDto);
 return Ok(resultDtos);
 }

 [HttpGet("{id}")]
 public async Task<ActionResult<DrawResultDto>> GetDrawResult(int id)
 {
 var result = await _drawResultRepository.GetByIdAsync(id);

 if (result == null)
 {
 return NotFound();
 }

 return Ok(_mappingService.MapToDto(result));
 }
}