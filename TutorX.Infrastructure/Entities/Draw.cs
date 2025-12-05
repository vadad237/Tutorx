namespace TutorX.Infrastructure.Entities;

public class Draw
{
 public int Id { get; set; }
 public string Name { get; set; } = null!;
 public int? ActivityId { get; set; } // Nullable if not tied to an activity
 public Activity? Activity { get; set; } // Navigation property
 public int? GroupId { get; set; } // Nullable if not tied to a group
 public StudentGroup? Group { get; set; } // Navigation property
 public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
 public ICollection<DrawResult> Results { get; set; } = new List<DrawResult>(); // Navigation property
}