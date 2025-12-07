using TutorX.Infrastructure.Entities;
using TutorX.Shared.DTOs;

namespace TutorX.Server.Services;

public class MappingService : IMappingService
{
    // Student mappings
    public StudentDto MapToDto(Student student)
    {
        return new StudentDto
        {
            Id = student.Id,
            Name = student.Name,
            Email = student.Email,
            CardNumber = student.CardNumber,
            Year = student.Year,
            GroupIds = student.Groups?.Select(g => g.Id).ToList() ?? new List<int>(),
            GroupNames = student.Groups?.Select(g => g.Name).ToList() ?? new List<string>()
        };
    }

    public Student MapToEntity(CreateStudentDto dto)
    {
        return new Student
        {
            Name = dto.Name,
            Email = dto.Email,
            CardNumber = dto.CardNumber,
            Year = dto.Year
            // Groups will be set separately in the controller
        };
    }

    public void MapToEntity(UpdateStudentDto dto, Student student)
    {
        student.Name = dto.Name;
        student.Email = dto.Email;
        student.CardNumber = dto.CardNumber;
        student.Year = dto.Year;
        // Groups will be updated separately in the controller
    }

    // StudentGroup mappings
    public StudentGroupDto MapToDto(StudentGroup group)
    {
        return new StudentGroupDto
        {
            Id = group.Id,
            Name = group.Name,
            Description = group.Description,
            Students = group.Students?.Select(MapToDto).ToList() ?? new List<StudentDto>()
        };
    }

    public StudentGroup MapToEntity(CreateStudentGroupDto dto)
    {
        return new StudentGroup
        {
            Name = dto.Name,
            Description = dto.Description
        };
    }

    public void MapToEntity(UpdateStudentGroupDto dto, StudentGroup group)
    {
        group.Name = dto.Name;
        group.Description = dto.Description;
    }

    // Activity mappings
    public ActivityDto MapToDto(Activity activity)
    {
        return new ActivityDto
        {
            Id = activity.Id,
            Name = activity.Name,
            Description = activity.Description,
            GroupId = activity.GroupId,
            GroupName = activity.Group?.Name,
            StudentIds = activity.Students?.Select(s => s.Id).ToList() ?? new List<int>(),
            StudentNames = activity.Students?.Select(s => s.Name).ToList() ?? new List<string>()
        };
    }

    public Activity MapToEntity(CreateActivityDto dto)
    {
        return new Activity
        {
            Name = dto.Name,
            Description = dto.Description,
            GroupId = dto.GroupId
            // Students will be set separately in the controller
        };
    }

    public void MapToEntity(UpdateActivityDto dto, Activity activity)
    {
        activity.Name = dto.Name;
        activity.Description = dto.Description;
        activity.GroupId = dto.GroupId;
        // Students will be updated separately in the controller
    }

    // ActivityAssignment mappings
    public ActivityAssignmentDto MapToDto(ActivityAssignment assignment)
    {
        return new ActivityAssignmentDto
        {
            Id = assignment.Id,
            ActivityId = assignment.ActivityId,
            ActivityName = assignment.Activity?.Name,
            StudentId = assignment.StudentId,
            StudentName = assignment.Student?.Name,
            IsCompleted = assignment.IsCompleted
        };
    }

    public ActivityAssignment MapToEntity(CreateActivityAssignmentDto dto)
    {
        return new ActivityAssignment
        {
            ActivityId = dto.ActivityId,
            StudentId = dto.StudentId,
            IsCompleted = dto.IsCompleted
        };
    }

    public void MapToEntity(UpdateActivityAssignmentDto dto, ActivityAssignment assignment)
    {
        assignment.ActivityId = dto.ActivityId;
        assignment.StudentId = dto.StudentId;
        assignment.IsCompleted = dto.IsCompleted;
    }

    // Draw mappings
    public DrawDto MapToDto(Draw draw)
    {
        return new DrawDto
        {
            Id = draw.Id,
            Name = draw.Name,
            ActivityId = draw.ActivityId,
            ActivityName = draw.Activity?.Name,
            GroupId = draw.GroupId,
            GroupName = draw.Group?.Name,
            CreatedAt = draw.CreatedAt,
            Results = draw.Results?.Select(MapToDto).ToList() ?? new List<DrawResultDto>()
        };
    }

    public Draw MapToEntity(CreateDrawDto dto)
    {
        return new Draw
        {
            Name = dto.Name,
            ActivityId = dto.ActivityId,
            GroupId = dto.GroupId,
            CreatedAt = DateTime.UtcNow
        };
    }

    // DrawResult mappings
    public DrawResultDto MapToDto(DrawResult result)
    {
        return new DrawResultDto
        {
            Id = result.Id,
            DrawId = result.DrawId,
            DrawName = result.Draw?.Name,
            StudentId = result.StudentId,
            StudentName = result.Student?.Name
        };
    }
}
