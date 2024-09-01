namespace NetASP.Authorization;

public class UserLoginRequest(string email, string password)
{
    public string Email { get; } = email;
    public string Password { get; } = password;
}