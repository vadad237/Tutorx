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
            GroupId = student.GroupId,
   GroupName = student.Group?.Name
   };
    }

    public Student MapToEntity(CreateStudentDto dto)
    {
return new Student
        {
            Name = dto.Name,
    Email = dto.Email,
  GroupId = dto.GroupId
        };
    }

    public void MapToEntity(UpdateStudentDto dto, Student student)
    {
        student.Name = dto.Name;
        student.Email = dto.Email;
        student.GroupId = dto.GroupId;
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
    GroupName = activity.Group?.Name
        };
    }

    public Activity MapToEntity(CreateActivityDto dto)
    {
   return new Activity
        {
 Name = dto.Name,
      Description = dto.Description,
            GroupId = dto.GroupId
        };
    }

    public void MapToEntity(UpdateActivityDto dto, Activity activity)
    {
        activity.Name = dto.Name;
        activity.Description = dto.Description;
      activity.GroupId = dto.GroupId;
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
