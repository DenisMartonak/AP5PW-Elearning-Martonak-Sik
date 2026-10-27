namespace PW.Elearning.Application.Courses;

public interface ICourseService
{
    Task<IReadOnlyList<CourseDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<int> CreateAsync(string title, string? description, int teacherId, CancellationToken cancellationToken = default);
}
