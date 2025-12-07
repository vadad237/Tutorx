namespace TutorX.Shared.DTOs;

public class StudentDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? CardNumber { get; set; }
    public int? Year { get; set; }
    public List<int> GroupIds { get; set; } = new();
    public List<string> GroupNames { get; set; } = new();
}

public class CreateStudentDto
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? CardNumber { get; set; }
    public int? Year { get; set; }
    public List<int> GroupIds { get; set; } = new();
}

public class UpdateStudentDto
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? CardNumber { get; set; }
    public int? Year { get; set; }
    public List<int> GroupIds { get; set; } = new();
}
