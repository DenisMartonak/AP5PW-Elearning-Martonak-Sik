namespace PW.Elearning.Domain.Entities.Interfaces;

public interface IUser
{
    string Username { get; set; }
    string FirstName { get; set; }
    string LastName { get; set; }
    string Email { get; set; }
    string PasswordHash { get; set; }
}
