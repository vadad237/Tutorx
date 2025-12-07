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

        // Configure many-to-many relationship between Student and StudentGroup
        modelBuilder.Entity<Student>()
      .HasMany(s => s.Groups)
        .WithMany(g => g.Students)
            .UsingEntity<Dictionary<string, object>>(
                "StudentGroupMembership",
         j => j.HasOne<StudentGroup>().WithMany().HasForeignKey("GroupId"),
   j => j.HasOne<Student>().WithMany().HasForeignKey("StudentId")
            );

     // Configure many-to-many relationship between Activity and Student
      modelBuilder.Entity<Activity>()
 .HasMany(a => a.Students)
        .WithMany()
   .UsingEntity<Dictionary<string, object>>(
 "ActivityStudentMembership",
           j => j.HasOne<Student>().WithMany().HasForeignKey("StudentId"),
         j => j.HasOne<Activity>().WithMany().HasForeignKey("ActivityId")
);
    }
}