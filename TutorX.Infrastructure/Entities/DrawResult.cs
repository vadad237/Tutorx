namespace TutorX.Infrastructure.Entities;

public class DrawResult
{
 public int Id { get; set; }
 public int DrawId { get; set; }
 public Draw Draw { get; set; } = null!; // Navigation property
 public int StudentId { get; set; }
 public Student Student { get; set; } = null!; // Navigation property
}