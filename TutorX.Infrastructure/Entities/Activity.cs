namespace TutorX.Infrastructure.Entities;

public class Activity
{
 public int Id { get; set; }
 public string Name { get; set; } = null!;
 public string? Description { get; set; }
 public int? GroupId { get; set; } // Nullable if no group is assigned
 public StudentGroup? Group { get; set; } // Navigation property
 public ICollection<Student> Students { get; set; } = new List<Student>(); // Many-to-many with students
 public ICollection<ActivityAssignment> Assignments { get; set; } = new List<ActivityAssignment>(); // Navigation property
 public DateTime? DueDate { get; set; }
}