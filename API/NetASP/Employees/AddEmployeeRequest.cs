namespace NetASP.Employees;

public class AddEmployeeRequest(string emailSession, string firstName, string lastName, string email, string password, string phoneNumber, string address) : BaseRequest(emailSession)
{
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;
    public string Email { get; set; } = email;
    public string Password { get; set; } = password;
    public string PhoneNumber { get; set; } = phoneNumber;
    public string Address { get; set; } = address;
}