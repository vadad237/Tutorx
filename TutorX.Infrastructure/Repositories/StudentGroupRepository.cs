using Microsoft.EntityFrameworkCore;
using TutorX.Infrastructure.Entities;

namespace TutorX.Infrastructure.Repositories;

public class StudentGroupRepository : Repository<StudentGroup>, IStudentGroupRepository
{
    public StudentGroupRepository(TutorXDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<StudentGroup>> GetGroupsWithStudentsAsync()
 {
        return await _context.StudentGroups.Include(g => g.Students).ToListAsync();
    }
}
