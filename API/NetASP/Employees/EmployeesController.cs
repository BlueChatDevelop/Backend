using Microsoft.AspNetCore.Mvc;

namespace NetASP.Employees;

[Route("api/[controller]")]
[ApiController]
public class EmployeesController(IHttpContextAccessor httpContextAccessor) : ControllerBase(httpContextAccessor)
{
    [HttpGet("[action]")]
    public IActionResult GetEmployees()
    {
        EnsureAuthenticated();

        using var reader = Get(["*"]);
        var employees = new List<Employee>();
        while (reader.Read())
        {
            employees.Add(
                new Employee
                (
                    reader.GetInt32("ID"),
                    reader.GetString("FirstName"),
                    reader.GetString("LastName"),
                    reader.GetString("Email"),
                    reader.GetString("Password"),
                    reader.GetString("PhoneNumber"),
                    reader.GetString("Address")
                )
            );
        }

        return Ok(employees);
    }

    [HttpPost("[action]")]
    public IActionResult AddEmployee(Employee newEmployee)
    {
        EnsureAuthenticated();

        var insertResult = Insert([
            ("FirstName", newEmployee.FirstName),
            ("LastName", newEmployee.LastName),
            ("Email", newEmployee.Email),
            ("Password", newEmployee.Password),
            ("PhoneNumber", newEmployee.PhoneNumber),
            ("Address", newEmployee.Address)
        ]);

        if (insertResult > 0)
        {
            return Ok("Employee added successfully.");
        }

        return BadRequest("Failed to add employee.");
    }

    [HttpPut("[action]/{id}")]
    public IActionResult UpdateEmployee(int id, Employee updatedEmployee)
    {
        EnsureAuthenticated();

        var updateResult = Update([
                ("FirstName", updatedEmployee.FirstName),
                ("LastName", updatedEmployee.LastName),
                ("Email", updatedEmployee.Email),
                ("PhoneNumber", updatedEmployee.PhoneNumber),
                ("Address", updatedEmployee.Address)
            ],
            [
                ("ID", id)
            ]);

        if (updateResult > 0)
        {
            return Ok("Employee updated successfully.");
        }

        return BadRequest("Failed to update employee.");
    }

    [HttpDelete("[action]/{id}")]
    public IActionResult DeleteEmployee(int id)
    {
        EnsureAuthenticated();

        var deleteResult = Delete([
            ("ID", id)
        ]);

        if (deleteResult > 0)
        {
            return Ok("Employee deleted successfully.");
        }

        return BadRequest("Failed to delete employee.");
    }
}