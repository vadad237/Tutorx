namespace TutorX.Infrastructure.Entities;

public class ActivityAssignment
{
 public int Id { get; set; }
 public int ActivityId { get; set; }
 public Activity Activity { get; set; } = null!; // Navigation property
 public int StudentId { get; set; }
 public Student Student { get; set; } = null!; // Navigation property
 public bool IsCompleted { get; set; } = false;
}