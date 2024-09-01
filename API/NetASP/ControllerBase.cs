using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace NetASP;

public abstract class ControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase
{
    protected readonly IHttpContextAccessor HttpContextAccessor;
    protected string TableName;

    protected ControllerBase(IHttpContextAccessor httpContextAccessor)
    {
        HttpContextAccessor = httpContextAccessor;

        TableName = GetType().Name.Replace("Controller", "");
    }

    private string TakeConnectionString()
    {
        var server = Environment.GetEnvironmentVariable("DATABASE_HOST");
        var database = Environment.GetEnvironmentVariable("DATABASE_NAME");
        var user = Environment.GetEnvironmentVariable("DATABASE_USER");
        var password = Environment.GetEnvironmentVariable("DATABASE_PASSWORD");

        var connectionString = $"Server={server};Database={database};User={user};Password={password};";
        return connectionString;
    }

    protected bool IsUserAuthenticated()
    {
        var email = HttpContext.Session.GetString("UserEmail");
        return !string.IsNullOrEmpty(email);
    }

    protected IActionResult EnsureAuthenticated()
    {
        if (!IsUserAuthenticated())
        {
            return Unauthorized("User is not logged in.");
        }

        return NotFound("User is not logged in.");
    }

    protected MySqlDataReader Get(string[] columns, (string, object)[]? filters = null)
    {
        var command = new MySqlCommand();
        command.Connection = new MySqlConnection(TakeConnectionString());

        string columnsString = string.Join(", ", columns);
        string query = $"SELECT {columnsString} FROM {TableName}";

        if (filters is { Length: > 0 })
        {
            var filterConditions = filters.Select(f => $"{f.Item1} = @{f.Item1}");
            query += " WHERE " + string.Join(" AND ", filterConditions);

            foreach (var filter in filters)
            {
                command.Parameters.AddWithValue($"@{filter.Item1}", filter.Item2);
            }
        }

        command.CommandText = query;
        return command.ExecuteReader();
    }

    protected int Delete((string, object)[] filters)
    {
        using var conn = new MySqlConnection(TakeConnectionString());
        conn.Open();
        if (filters == null || filters.Length == 0)
        {
            throw new ArgumentException("At least one filter condition must be provided for DELETE operation.");
        }

        var filterConditions = filters.Select(f => $"{f.Item1} = @{f.Item1}");
        string query = $"DELETE FROM {TableName} WHERE {string.Join(" AND ", filterConditions)}";

        var command = new MySqlCommand(query, conn);

        foreach (var filter in filters)
        {
            command.Parameters.AddWithValue($"@{filter.Item1}", filter.Item2);
        }

        return command.ExecuteNonQuery();
    }

    protected int Insert((string, object)[] parameters)
    {
        using var conn = new MySqlConnection(TakeConnectionString());
        conn.Open();

        var columnNames = parameters.Select(p => p.Item1);
        var parameterNames = parameters.Select(p => $"@{p.Item1}");

        string columnsString = string.Join(", ", columnNames);
        string parametersString = string.Join(", ", parameterNames);

        string query = $"INSERT INTO {TableName} ({columnsString}) VALUES ({parametersString})";

        var command = new MySqlCommand(query, conn);

        foreach (var parameter in parameters)
        {
            command.Parameters.AddWithValue($"@{parameter.Item1}", parameter.Item2);
        }

        return command.ExecuteNonQuery();
    }

    protected int Update((string, object)[] parameters, (string, object)[] filters)
    {
        using var conn = new MySqlConnection(TakeConnectionString());
        conn.Open();

        if (filters == null || filters.Length == 0)
        {
            throw new ArgumentException("At least one filter condition must be provided for UPDATE operation.");
        }

        var updateStatements = parameters.Select(p => $"{p.Item1} = @{p.Item1}");
        var filterConditions = filters.Select(f => $"{f.Item1} = @{f.Item1}_filter");

        string updateString = string.Join(", ", updateStatements);
        string filterString = string.Join(" AND ", filterConditions);

        string query = $"UPDATE {TableName} SET {updateString} WHERE {filterString}";

        var command = new MySqlCommand(query, conn);

        foreach (var parameter in parameters)
        {
            command.Parameters.AddWithValue($"@{parameter.Item1}", parameter.Item2);
        }

        foreach (var filter in filters)
        {
            command.Parameters.AddWithValue($"@{filter.Item1}_filter", filter.Item2);
        }

        return command.ExecuteNonQuery();
    }
}