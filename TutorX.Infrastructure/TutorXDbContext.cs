using Microsoft.EntityFrameworkCore;
using TutorX.Infrastructure.Entities;

namespace TutorX.Infrastructure;

public class TutorXDbContext : DbContext
{
    public TutorXDbContext(DbContextOptions<TutorXDbContext> options) : base(options) { }

    public DbSet<Student> Students { get; set; } = null!;
    public DbSet<StudentGroup> StudentGroups { get; set; } = null!;
    public DbSet<Activity> Activities { get; set; } = null!;
    public DbSet<ActivityAssignment> ActivityAssignments { get; set; } = null!;
    public DbSet<Draw> Draws { get; set; } = null!;
    public DbSet<DrawResult> DrawResults { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

    }
}