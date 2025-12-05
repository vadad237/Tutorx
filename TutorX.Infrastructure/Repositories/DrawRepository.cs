using Microsoft.EntityFrameworkCore;
using TutorX.Infrastructure.Entities;

namespace TutorX.Infrastructure.Repositories;

public class DrawRepository : Repository<Draw>, IDrawRepository
{
    public DrawRepository(TutorXDbContext context) : base(context)
  {
    }

    public async Task<IEnumerable<Draw>> GetDrawsWithDetailsAsync()
    {
        return await _context.Draws
        .Include(d => d.Group)
  .Include(d => d.Activity)
 .ToListAsync();
    }

    public async Task<Draw?> GetDrawWithResultsAsync(int id)
    {
        return await _context.Draws
 .Include(d => d.Group)
            .Include(d => d.Activity)
         .Include(d => d.Results)
 .ThenInclude(dr => dr.Student)
          .FirstOrDefaultAsync(d => d.Id == id);
    }
}
