using TutorX.Infrastructure.Entities;

namespace TutorX.Infrastructure.Repositories;

public interface IActivityRepository : IRepository<Activity>
{
    Task<IEnumerable<Activity>> GetActivitiesWithGroupsAsync();
    Task<Activity?> GetActivityWithAssignmentsAsync(int id);
}
