namespace TutorX.Shared.DTOs;

public class StudentGroupDto
{
    public int Id { get; set; }
  public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public List<StudentDto> Students { get; set; } = new();
}

public class CreateStudentGroupDto
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}

public class UpdateStudentGroupDto
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}
