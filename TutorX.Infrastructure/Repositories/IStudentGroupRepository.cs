using TutorX.Infrastructure.Entities;

namespace TutorX.Infrastructure.Repositories;

public interface IStudentGroupRepository : IRepository<StudentGroup>
{
    Task<IEnumerable<StudentGroup>> GetGroupsWithStudentsAsync();
}
