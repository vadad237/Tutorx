using Microsoft.EntityFrameworkCore;
using TutorX.Infrastructure.Entities;

namespace TutorX.Infrastructure.Repositories;

public class ActivityRepository : Repository<Activity>, IActivityRepository
{
    public ActivityRepository(TutorXDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Activity>> GetActivitiesWithGroupsAsync()
    {
        return await _context.Activities.Include(a => a.Group).ToListAsync();
    }

    public async Task<Activity?> GetActivityWithAssignmentsAsync(int id)
    {
   return await _context.Activities
      .Include(a => a.Group)
   .Include(a => a.Assignments)
            .ThenInclude(aa => aa.Student)
         .FirstOrDefaultAsync(a => a.Id == id);
    }
}
