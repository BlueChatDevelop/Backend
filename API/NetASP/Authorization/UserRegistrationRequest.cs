namespace NetASP.Authorization;

public class UserRegistrationRequest(string name, string surname, string email, string password)
{
    public string Name { get; } = name;
    public string Surname { get; } = surname;
    public string Email { get; } = email;
    public string Password { get; } = password;
}