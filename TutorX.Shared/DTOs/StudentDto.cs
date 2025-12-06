namespace TutorX.Shared.DTOs;

public class StudentDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? CardNumber { get; set; }
    public int? Year { get; set; }
    public int? GroupId { get; set; }
    public string? GroupName { get; set; }
}

public class CreateStudentDto
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? CardNumber { get; set; }
    public int? Year { get; set; }
    public int? GroupId { get; set; }
}

public class UpdateStudentDto
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? CardNumber { get; set; }
    public int? Year { get; set; }
    public int? GroupId { get; set; }
}
