using Microsoft.AspNetCore.Mvc;

namespace NetASP.Authorization;

[Route("api/[controller]")]
[ApiController]
public class AuthorizationController : ControllerBase
{
    public AuthorizationController(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        TableName = "Users";
    }

    [HttpGet("check")]
    public IActionResult CheckEmail([FromQuery] string email)
    {
        EnsureAuthenticated();

        try
        {
            bool exists;
            using (var reader = Get(["COUNT(1)"], [("Email", email)]))
            {
                exists = reader.HasRows;
            }

            return Ok(exists);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return StatusCode(500, "Internal server error occurred.");
        }
    }

    [HttpPost("reg")]
    public IActionResult Registration([FromBody] UserRegistrationRequest request)
    {
        var insertResult = Insert([
            ("Name", request.Name),
            ("Surname", request.Surname),
            ("Email", request.Email),
            ("Password", request.Password)
        ]);
        if (insertResult > 0)
        {
            HttpContextAccessor.HttpContext?.Session.SetString("UserEmail", request.Email);
            return Ok(true);
        }

        return BadRequest("Registration failed");
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] UserLoginRequest request)
    {
        bool isValidUser = Get(["COUNT(1)"], [("Email", request.Email), ("Password", request.Password)]).FieldCount > 0;

        if (isValidUser)
        {
            HttpContextAccessor.HttpContext?.Session.SetString("UserEmail", request.Email);
            return Ok(true);
        }

        return Unauthorized();
    }


    [HttpDelete("delete")]
    public IActionResult DeleteUser()
    {
        EnsureAuthenticated();

        var reader = Delete([("Email", HttpContext.Session.GetString("UserEmail"))]);
        HttpContext.Session.Clear();
        return Ok(reader > 0);
    }

    [HttpGet("getuserinfo")]
    public IActionResult GetUserInfo()
    {
        EnsureAuthenticated();

        var email = HttpContext.Session.GetString("UserEmail");

        using (var reader = Get(["Name", "Surname", "Email", "Password"], [("Email", email)!]))

            if (reader.Read())
            {
                var userInfo = new
                {
                    Name = reader["Name"].ToString(),
                    Surname = reader["Surname"].ToString(),
                    Email = reader["Email"].ToString(),
                    Password = reader["Password"].ToString()
                };

                return Ok(userInfo);
            }

        return NotFound("User not found.");
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return Ok("User logged out successfully.");
    }

    [HttpGet("isAuthenticated")]
    public IActionResult IsAuthenticated()
    {
        return Ok(IsUserAuthenticated());
    }
}