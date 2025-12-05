using TutorX.Infrastructure.Entities;

namespace TutorX.Infrastructure.Repositories;

public interface IDrawResultRepository : IRepository<DrawResult>
{
    Task<IEnumerable<DrawResult>> GetResultsWithDetailsAsync();
}
