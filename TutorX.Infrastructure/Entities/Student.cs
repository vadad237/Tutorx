namespace TutorX.Infrastructure.Entities;

public class Student
{
 public int Id { get; set; }
 public string Name { get; set; } = null!;
 public string Email { get; set; } = null!;
 public int? GroupId { get; set; } // Nullable if no group is assigned
 public StudentGroup? Group { get; set; } // Navigation property
}