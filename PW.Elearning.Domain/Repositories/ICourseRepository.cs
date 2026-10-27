using PW.Elearning.Domain.Entities;

namespace PW.Elearning.Domain.Repositories;

public interface ICourseRepository : IRepository<Course>
{
    Task<IReadOnlyList<Course>> GetAllWithTeacherAsync(CancellationToken cancellationToken = default);
    Task<Course?> GetWithLessonsAsync(int id, CancellationToken cancellationToken = default);
}
