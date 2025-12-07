using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using TutorX.Infrastructure.Entities;
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

    [HttpPost("import")]
    public async Task<ActionResult<ImportResult>> ImportStudents(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest(new ImportResult
            {
                Success = false,
                Message = "No file uploaded"
            });
        }

        if (!file.FileName.EndsWith(".xlsx") && !file.FileName.EndsWith(".xls"))
        {
            return BadRequest(new ImportResult
            {
                Success = false,
                Message = "Only Excel files (.xlsx, .xls) are supported"
            });
        }

        var result = new ImportResult
        {
            Success = true,
            Message = "Import completed",
            Errors = new List<string>()
        };

        try
        {
            using var stream = file.OpenReadStream();
            using var workbook = new XLWorkbook(stream);
            var worksheet = workbook.Worksheets.First();

            // Find the header row (looking for "Meno" column)
            var headerRow = 0;
            for (int row = 1; row <= 10; row++)
            {
                var cellValue = worksheet.Cell(row, 1).GetString();
                if (cellValue == "Meno")
                {
                    headerRow = row;
                    break;
                }
            }

            if (headerRow == 0)
            {
                return BadRequest(new ImportResult
                {
                    Success = false,
                    Message = "Could not find header row with 'Meno' column"
                });
            }

            // Find column indices
            var menoCol = 1;  // Meno
            var priezviskoCol = 2;  // Priezvisko
            var cisloKartyCol = -1;  // Číslo karty
            var rocnikCol = -1;  // Ročník

            // Search for required columns
            for (int col = 1; col <= worksheet.ColumnsUsed().Count(); col++)
            {
                var header = worksheet.Cell(headerRow, col).GetString().Trim();
                if (header == "Číslo karty")
                    cisloKartyCol = col;
                else if (header == "Ročník")
                    rocnikCol = col;
            }

            // Process each row
            int currentRow = headerRow + 1;
            while (!worksheet.Cell(currentRow, menoCol).IsEmpty())
            {
                try
                {
                    var meno = worksheet.Cell(currentRow, menoCol).GetString().Trim();
                    var priezvisko = worksheet.Cell(currentRow, priezviskoCol).GetString().Trim();

                    if (string.IsNullOrWhiteSpace(meno) || string.IsNullOrWhiteSpace(priezvisko))
                    {
                        currentRow++;
                        continue;
                    }

                    var fullName = $"{meno} {priezvisko}";

                    // Get card number (optional)
                    string? cardNumber = null;
                    if (cisloKartyCol > 0 && !worksheet.Cell(currentRow, cisloKartyCol).IsEmpty())
                    {
                        cardNumber = worksheet.Cell(currentRow, cisloKartyCol).GetString().Trim();
                    }

                    // Get year (optional)
                    int? year = null;
                    if (rocnikCol > 0 && !worksheet.Cell(currentRow, rocnikCol).IsEmpty())
                    {
                        var rocnikValue = worksheet.Cell(currentRow, rocnikCol).GetString().Trim();
                        if (int.TryParse(rocnikValue, out int parsedYear))
                        {
                            year = parsedYear;
                        }
                    }

                    // Create student
                    var student = new Student
                    {
                        Name = fullName,
                        Email = $"{meno.ToLower()}.{priezvisko.ToLower()}@example.com", // Generate default email
                        CardNumber = cardNumber,
                        Year = year
                    };

                    await _studentRepository.AddAsync(student);
                    result.ImportedCount++;
                }
                catch (Exception ex)
                {
                    result.FailedCount++;
                    result.Errors.Add($"Row {currentRow}: {ex.Message}");
                }

                currentRow++;
            }

            if (result.FailedCount > 0)
            {
                result.Message = $"Import completed with {result.FailedCount} errors";
            }
            else
            {
                result.Message = $"Successfully imported {result.ImportedCount} students";
            }
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.Message = $"Import failed: {ex.Message}";
            result.Errors.Add(ex.ToString());
        }
        await _studentRepository.SaveChangesAsync();
        return Ok(result);
    }
}

public class ImportResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public int ImportedCount { get; set; }
    public int FailedCount { get; set; }
    public List<string> Errors { get; set; } = new();
}