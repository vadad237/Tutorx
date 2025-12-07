namespace TutorX.Shared.DTOs;

public class DrawDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int? ActivityId { get; set; }
    public string? ActivityName { get; set; }
    public int? GroupId { get; set; }
    public string? GroupName { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<DrawResultDto> Results { get; set; } = new();
}

public class CreateDrawDto
{
    public string Name { get; set; } = null!;
    public int? ActivityId { get; set; }
    public int? GroupId { get; set; }
    public List<int> StudentIds { get; set; } = new();
    public int NumberToSelect { get; set; } = 1;
}
