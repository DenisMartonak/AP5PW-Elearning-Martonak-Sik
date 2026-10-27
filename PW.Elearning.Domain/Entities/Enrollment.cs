namespace PW.Elearning.Domain.Entities;

public class Enrollment : BaseEntity
{
    public int StudentId { get; set; }
    public User Student { get; set; } = null!;

    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;
}
