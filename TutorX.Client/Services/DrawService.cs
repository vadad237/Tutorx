using System.Net.Http.Json;
using TutorX.Shared.DTOs;

namespace TutorX.Client.Services;

public interface IDrawService
{
    Task<List<DrawDto>> GetDrawsAsync();
    Task<DrawDto?> GetDrawAsync(int id);
    Task<DrawDto> CreateDrawAsync(CreateDrawDto draw);
    Task DeleteDrawAsync(int id);
}

public class DrawService : IDrawService
{
    private readonly HttpClient _httpClient;

 public DrawService(HttpClient httpClient)
    {
  _httpClient = httpClient;
  }

    public async Task<List<DrawDto>> GetDrawsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<DrawDto>>("api/draws") ?? new List<DrawDto>();
    }

    public async Task<DrawDto?> GetDrawAsync(int id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<DrawDto>($"api/draws/{id}");
        }
    catch (Exception ex)
        {
       Console.WriteLine($"Error getting draw {id}: {ex.Message}");
            throw;
        }
    }

    public async Task<DrawDto> CreateDrawAsync(CreateDrawDto draw)
    {
 var response = await _httpClient.PostAsJsonAsync("api/draws", draw);
      response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<DrawDto>() ?? throw new Exception("Failed to create draw");
    }

  public async Task DeleteDrawAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/draws/{id}");
        response.EnsureSuccessStatusCode();
    }
}
