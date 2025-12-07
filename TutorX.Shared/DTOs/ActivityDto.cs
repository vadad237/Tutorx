namespace TutorX.Shared.DTOs;

public class ActivityDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int? GroupId { get; set; }
    public string? GroupName { get; set; }
    public List<int> StudentIds { get; set; } = new();
    public List<string> StudentNames { get; set; } = new();
}

public class CreateActivityDto
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int? GroupId { get; set; }
    public List<int> StudentIds { get; set; } = new();
}

public class UpdateActivityDto
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int? GroupId { get; set; }
    public List<int> StudentIds { get; set; } = new();
}
