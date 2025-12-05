using Microsoft.EntityFrameworkCore;
using TutorX.Infrastructure.Entities;

namespace TutorX.Infrastructure.Repositories;

public class ActivityAssignmentRepository : Repository<ActivityAssignment>, IActivityAssignmentRepository
{
    public ActivityAssignmentRepository(TutorXDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ActivityAssignment>> GetAssignmentsWithDetailsAsync()
    {
        return await _context.ActivityAssignments
            .Include(aa => aa.Activity)
         .Include(aa => aa.Student)
            .ToListAsync();
    }
}
