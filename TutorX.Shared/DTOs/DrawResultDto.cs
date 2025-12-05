namespace TutorX.Shared.DTOs;

public class DrawResultDto
{
    public int Id { get; set; }
    public int DrawId { get; set; }
    public string? DrawName { get; set; }
    public int StudentId { get; set; }
    public string? StudentName { get; set; }
}
