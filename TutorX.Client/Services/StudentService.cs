using System.Net.Http.Json;
using TutorX.Shared.DTOs;

namespace TutorX.Client.Services;

public interface IStudentService
{
    Task<List<StudentDto>> GetStudentsAsync();
    Task<StudentDto?> GetStudentAsync(int id);
    Task<StudentDto> CreateStudentAsync(CreateStudentDto student);
    Task UpdateStudentAsync(int id, UpdateStudentDto student);
    Task DeleteStudentAsync(int id);
    Task<ImportResult> ImportStudentsFromExcelAsync(byte[] fileData);
}

public class StudentService : IStudentService
{
    private readonly HttpClient _httpClient;

    public StudentService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<StudentDto>> GetStudentsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/students");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<StudentDto>>() ?? new List<StudentDto>();
            }
            else
            {
                Console.WriteLine($"API returned status code: {response.StatusCode}");
                return new List<StudentDto>();
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Request error: {ex.Message}");
            return new List<StudentDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching students: {ex.Message}");
            return new List<StudentDto>();
        }
    }

    public async Task<StudentDto?> GetStudentAsync(int id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<StudentDto>($"api/students/{id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching student {id}: {ex.Message}");
            return null;
        }
    }

    public async Task<StudentDto> CreateStudentAsync(CreateStudentDto student)
    {
        var response = await _httpClient.PostAsJsonAsync("api/students", student);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<StudentDto>() ?? throw new Exception("Failed to create student");
    }

    public async Task UpdateStudentAsync(int id, UpdateStudentDto student)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/students/{id}", student);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteStudentAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/students/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<ImportResult> ImportStudentsFromExcelAsync(byte[] fileData)
    {
        using var content = new MultipartFormDataContent();
        content.Add(new ByteArrayContent(fileData), "file", "import.xlsx");

        var response = await _httpClient.PostAsync("api/students/import", content);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<ImportResult>()
            ?? new ImportResult { Success = false, Message = "No response from server" };
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