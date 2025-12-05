using Microsoft.EntityFrameworkCore;
using TutorX.Infrastructure.Entities;

namespace TutorX.Infrastructure.Repositories;

public class DrawResultRepository : Repository<DrawResult>, IDrawResultRepository
{
    public DrawResultRepository(TutorXDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<DrawResult>> GetResultsWithDetailsAsync()
    {
        return await _context.DrawResults
            .Include(dr => dr.Draw)
   .Include(dr => dr.Student)
            .ToListAsync();
    }
}
