using Microsoft.EntityFrameworkCore;
using PW.Elearning.Domain.Entities;
using PW.Elearning.Domain.Repositories;
using PW.Elearning.Infrastructure.Persistence;

namespace PW.Elearning.Infrastructure.Repositories;

public class CourseRepository(ElearningDbContext context) : Repository<Course>(context), ICourseRepository
{
    public async Task<IReadOnlyList<Course>> GetAllWithTeacherAsync(CancellationToken cancellationToken = default)
        => await DbSet.AsNoTracking()
            .Include(c => c.Teacher)
            .OrderBy(c => c.Title)
            .ToListAsync(cancellationToken);

    public async Task<Course?> GetWithLessonsAsync(int id, CancellationToken cancellationToken = default)
        => await DbSet
            .Include(c => c.Teacher)
            .Include(c => c.Lessons.OrderBy(l => l.Order))
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
}
