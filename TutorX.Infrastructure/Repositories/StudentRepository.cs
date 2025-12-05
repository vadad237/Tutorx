using Microsoft.EntityFrameworkCore;
using TutorX.Infrastructure.Entities;

namespace TutorX.Infrastructure.Repositories;

public class StudentRepository : Repository<Student>, IStudentRepository
{
    public StudentRepository(TutorXDbContext context) : base(context)
    {
  }

    public async Task<IEnumerable<Student>> GetStudentsWithGroupsAsync()
    {
        return await _context.Students.Include(s => s.Group).ToListAsync();
 }
}
