using TutorX.Infrastructure.Entities;

namespace TutorX.Infrastructure.Repositories;

public interface IDrawRepository : IRepository<Draw>
{
    Task<IEnumerable<Draw>> GetDrawsWithDetailsAsync();
    Task<Draw?> GetDrawWithResultsAsync(int id);
}
