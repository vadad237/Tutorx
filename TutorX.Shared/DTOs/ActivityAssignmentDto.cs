namespace TutorX.Shared.DTOs;

public class ActivityAssignmentDto
{
    public int Id { get; set; }
    public int ActivityId { get; set; }
    public string? ActivityName { get; set; }
    public int StudentId { get; set; }
    public string? StudentName { get; set; }
    public bool IsCompleted { get; set; }
}

public class CreateActivityAssignmentDto
{
    public int ActivityId { get; set; }
 public int StudentId { get; set; }
    public bool IsCompleted { get; set; }
}

public class UpdateActivityAssignmentDto
{
 public int ActivityId { get; set; }
    public int StudentId { get; set; }
    public bool IsCompleted { get; set; }
}
