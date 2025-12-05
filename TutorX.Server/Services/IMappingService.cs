using TutorX.Infrastructure.Entities;
using TutorX.Shared.DTOs;

namespace TutorX.Server.Services;

public interface IMappingService
{
    StudentDto MapToDto(Student student);
    Student MapToEntity(CreateStudentDto dto);
    void MapToEntity(UpdateStudentDto dto, Student student);
    
    StudentGroupDto MapToDto(StudentGroup group);
StudentGroup MapToEntity(CreateStudentGroupDto dto);
    void MapToEntity(UpdateStudentGroupDto dto, StudentGroup group);
    
    ActivityDto MapToDto(Activity activity);
  Activity MapToEntity(CreateActivityDto dto);
    void MapToEntity(UpdateActivityDto dto, Activity activity);
    
    ActivityAssignmentDto MapToDto(ActivityAssignment assignment);
    ActivityAssignment MapToEntity(CreateActivityAssignmentDto dto);
    void MapToEntity(UpdateActivityAssignmentDto dto, ActivityAssignment assignment);
    
    DrawDto MapToDto(Draw draw);
    Draw MapToEntity(CreateDrawDto dto);
    
    DrawResultDto MapToDto(DrawResult result);
}
