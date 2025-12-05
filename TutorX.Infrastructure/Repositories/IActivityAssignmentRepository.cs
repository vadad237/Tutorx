using TutorX.Infrastructure.Entities;

namespace TutorX.Infrastructure.Repositories;

public interface IActivityAssignmentRepository : IRepository<ActivityAssignment>
{
    Task<IEnumerable<ActivityAssignment>> GetAssignmentsWithDetailsAsync();
}
