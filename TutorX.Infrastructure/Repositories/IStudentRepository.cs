using TutorX.Infrastructure.Entities;

namespace TutorX.Infrastructure.Repositories;

public interface IStudentRepository : IRepository<Student>
{
    Task<IEnumerable<Student>> GetStudentsWithGroupsAsync();
}
