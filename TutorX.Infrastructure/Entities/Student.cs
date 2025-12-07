namespace TutorX.Infrastructure.Entities;

public class Student
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? CardNumber { get; set; }
    public int? Year { get; set; }
    public ICollection<StudentGroup> Groups { get; set; } = new List<StudentGroup>(); // Navigation property for many-to-many
}