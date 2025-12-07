using System.Net.Http.Json;
using TutorX.Shared.DTOs;

namespace TutorX.Client.Services;

public interface IStudentGroupService
{
    Task<List<StudentGroupDto>> GetStudentGroupsAsync();
    Task<StudentGroupDto?> GetStudentGroupAsync(int id);
    Task<StudentGroupDto> CreateStudentGroupAsync(CreateStudentGroupDto group);
    Task UpdateStudentGroupAsync(int id, UpdateStudentGroupDto group);
    Task DeleteStudentGroupAsync(int id);
    Task AddStudentToGroupAsync(int groupId, int studentId);
    Task RemoveStudentFromGroupAsync(int groupId, int studentId);
}

public class StudentGroupService : IStudentGroupService
{
    private readonly HttpClient _httpClient;

    public StudentGroupService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<StudentGroupDto>> GetStudentGroupsAsync()
    {
  return await _httpClient.GetFromJsonAsync<List<StudentGroupDto>>("api/studentgroups") ?? new List<StudentGroupDto>();
    }

    public async Task<StudentGroupDto?> GetStudentGroupAsync(int id)
    {
        try
    {
            return await _httpClient.GetFromJsonAsync<StudentGroupDto>($"api/studentgroups/{id}");
}
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting student group {id}: {ex.Message}");
            throw;
}
    }

    public async Task<StudentGroupDto> CreateStudentGroupAsync(CreateStudentGroupDto group)
    {
        var response = await _httpClient.PostAsJsonAsync("api/studentgroups", group);
        response.EnsureSuccessStatusCode();
   return await response.Content.ReadFromJsonAsync<StudentGroupDto>() ?? throw new Exception("Failed to create student group");
    }

    public async Task UpdateStudentGroupAsync(int id, UpdateStudentGroupDto group)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/studentgroups/{id}", group);
     response.EnsureSuccessStatusCode();
    }

    public async Task DeleteStudentGroupAsync(int id)
    {
var response = await _httpClient.DeleteAsync($"api/studentgroups/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task AddStudentToGroupAsync(int groupId, int studentId)
    {
    var response = await _httpClient.PostAsync($"api/studentgroups/{groupId}/students/{studentId}", null);
        response.EnsureSuccessStatusCode();
    }

public async Task RemoveStudentFromGroupAsync(int groupId, int studentId)
    {
        var response = await _httpClient.DeleteAsync($"api/studentgroups/{groupId}/students/{studentId}");
        response.EnsureSuccessStatusCode();
    }
}
