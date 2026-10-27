using PW.Elearning.Domain.Entities.Enums;
using PW.Elearning.Domain.Entities.Interfaces;

namespace PW.Elearning.Domain.Entities;

public class User : BaseEntity, IUser
{
    public string Username { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public Role Role { get; set; } = Role.Student;

    public ICollection<Course> TaughtCourses { get; set; } = new List<Course>();
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public ICollection<Submission> Submissions { get; set; } = new List<Submission>();
}
