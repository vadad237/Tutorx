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
            var test = await _httpClient.GetAsync("api/students");
        }
        catch (Exception ex)
        {
            Console.WriteLine();
        }

        return await _httpClient.GetFromJsonAsync<List<StudentDto>>("api/students") ?? new List<StudentDto>();
    }

    public async Task<StudentDto?> GetStudentAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<StudentDto>($"api/students/{id}");
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
}
