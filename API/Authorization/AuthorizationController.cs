using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace Authorization
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private string TakeConnectionString()
        {
            var server = Environment.GetEnvironmentVariable("DATABASE_HOST");
            var database = Environment.GetEnvironmentVariable("DATABASE_NAME");
            var user = Environment.GetEnvironmentVariable("DATABASE_USER");
            var password = Environment.GetEnvironmentVariable("DATABASE_PASSWORD");

            var connectionString = $"Server={server};Database={database};User={user};Password={password};";
            return connectionString;
        }

        [HttpGet("check")]
        public IActionResult CheckEmail([FromQuery] string email)
        {
            try
            {
                bool exists = false;
                using (var connection = new MySqlConnection(TakeConnectionString()))
                {
                    connection.Open();
                    
                    var command = new MySqlCommand("SELECT COUNT(1) FROM Users WHERE Email = @Email", connection);
                    command.Parameters.AddWithValue("@Email", email);

                    exists = Convert.ToInt32(command.ExecuteScalar()) > 0;
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
            using (var connection = new MySqlConnection(TakeConnectionString()))
            {
                connection.Open();
                var command = new MySqlCommand("INSERT INTO Users (Name, Surname, Email, Password) VALUES (@Name, @Surname, @Email, @Password)", connection);
                command.Parameters.AddWithValue("@Name", request.Name);
                command.Parameters.AddWithValue("@Surname", request.Surname);
                command.Parameters.AddWithValue("@Email", request.Email);
                command.Parameters.AddWithValue("@Password", request.Password);

                var result = command.ExecuteNonQuery();
                return Ok(result > 0);
            }
        }

        [HttpDelete("delete")]
        public IActionResult DeleteUser([FromBody] UserDeleteRequest request)
        {
            using (var connection = new MySqlConnection(TakeConnectionString()))
            {
                connection.Open();

                var query = "DELETE FROM Users WHERE Email = @Email AND Password = @Password";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", request.Email);
                    command.Parameters.AddWithValue("@Password", request.Password);

                    int rowsAffected = command.ExecuteNonQuery();

                    return Ok(rowsAffected > 0);
                }
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginRequest request)
        {
            bool isValidUser = false;
            using (var connection = new MySqlConnection(TakeConnectionString()))
            {
                connection.Open();
                var command =
                    new MySqlCommand("SELECT COUNT(1) FROM Users WHERE Email = @Email AND Password = @Password",
                        connection);
                command.Parameters.AddWithValue("@Email", request.Email);
                command.Parameters.AddWithValue("@Password", request.Password);

                isValidUser = Convert.ToInt32(command.ExecuteScalar()) > 0;
            }

            return Ok(isValidUser);
        }
    }

    public class UserRegistrationRequest
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UserLoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class UserDeleteRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}