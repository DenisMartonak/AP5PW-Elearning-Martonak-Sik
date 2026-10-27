namespace PW.Elearning.Domain.Entities;

public class Submission : BaseEntity
{
    public string? Content { get; set; }
    public int? Points { get; set; }
    public string? Feedback { get; set; }

    public int AssignmentId { get; set; }
    public Assignment Assignment { get; set; } = null!;

    public int StudentId { get; set; }
    public User Student { get; set; } = null!;
}
