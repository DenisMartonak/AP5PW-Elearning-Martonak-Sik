using PW.Elearning.Domain.Entities;
using PW.Elearning.Domain.Repositories;

namespace PW.Elearning.Application.Courses;

public class CourseService(ICourseRepository courses, IUnitOfWork unitOfWork) : ICourseService
{
    public async Task<IReadOnlyList<CourseDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var all = await courses.GetAllWithTeacherAsync(cancellationToken);
        return all
            .Select(c => new CourseDto(c.Id, c.Title, c.Description, $"{c.Teacher.FirstName} {c.Teacher.LastName}"))
            .ToList();
    }

    public async Task<int> CreateAsync(string title, string? description, int teacherId, CancellationToken cancellationToken = default)
    {
        var course = new Course { Title = title, Description = description, TeacherId = teacherId };
        await courses.AddAsync(course, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return course.Id;
    }
}
