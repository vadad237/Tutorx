using System.Net.Http.Json;
using TutorX.Shared.DTOs;

namespace TutorX.Client.Services;

public interface IActivityService
{
    Task<List<ActivityDto>> GetActivitiesAsync();
    Task<ActivityDto?> GetActivityAsync(int id);
    Task<ActivityDto> CreateActivityAsync(CreateActivityDto activity);
    Task UpdateActivityAsync(int id, UpdateActivityDto activity);
    Task DeleteActivityAsync(int id);
    Task AddStudentToActivityAsync(int activityId, int studentId);
    Task RemoveStudentFromActivityAsync(int activityId, int studentId);
}

public class ActivityService : IActivityService
{
    private readonly HttpClient _httpClient;

    public ActivityService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<ActivityDto>> GetActivitiesAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<ActivityDto>>("api/activities") ?? new List<ActivityDto>();
    }

    public async Task<ActivityDto?> GetActivityAsync(int id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<ActivityDto>($"api/activities/{id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting activity {id}: {ex.Message}");
            throw;
        }
    }

    public async Task<ActivityDto> CreateActivityAsync(CreateActivityDto activity)
    {
        var response = await _httpClient.PostAsJsonAsync("api/activities", activity);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ActivityDto>() ?? throw new Exception("Failed to create activity");
    }

    public async Task UpdateActivityAsync(int id, UpdateActivityDto activity)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/activities/{id}", activity);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteActivityAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/activities/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task AddStudentToActivityAsync(int activityId, int studentId)
    {
        var response = await _httpClient.PostAsync($"api/activities/{activityId}/students/{studentId}", null);
        response.EnsureSuccessStatusCode();
    }

    public async Task RemoveStudentFromActivityAsync(int activityId, int studentId)
    {
        var response = await _httpClient.DeleteAsync($"api/activities/{activityId}/students/{studentId}");
        response.EnsureSuccessStatusCode();
    }
}
