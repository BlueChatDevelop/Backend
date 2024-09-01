namespace NetASP.Employees;

public class Employee(
    int id,
    string firstName,
    string lastName,
    string email,
    string password,
    string phoneNumber,
    string address)
{

    public int Id { get; set; } = id;
    public string FirstName { get; } = firstName;
    public string LastName { get; } = lastName;
    public string Email { get; } = email;
    public string Password { get; } = password;
    public string PhoneNumber { get; } = phoneNumber;
    public string Address { get; } = address;
}