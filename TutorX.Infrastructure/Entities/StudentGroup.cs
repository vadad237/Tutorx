namespace TutorX.Infrastructure.Entities;

public class StudentGroup
{
 public int Id { get; set; }
 public string Name { get; set; } = null!;
 public string? Description { get; set; }
 public ICollection<Student> Students { get; set; } = new List<Student>(); // Navigation property
}