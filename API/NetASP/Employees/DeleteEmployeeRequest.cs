namespace NetASP.Employees;

public class DeleteEmployeeRequest(string emailSession, int id) : BaseRequest(emailSession)
{
    public int Id { get; set; } = id;
}