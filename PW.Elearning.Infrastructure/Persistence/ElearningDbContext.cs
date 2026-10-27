using Microsoft.EntityFrameworkCore;
using PW.Elearning.Domain.Entities;
using PW.Elearning.Domain.Repositories;

namespace PW.Elearning.Infrastructure.Persistence;

public class ElearningDbContext(DbContextOptions<ElearningDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Lesson> Lessons => Set<Lesson>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
    public DbSet<Assignment> Assignments => Set<Assignment>();
    public DbSet<Submission> Submissions => Set<Submission>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ElearningDbContext).Assembly);
    }
}
