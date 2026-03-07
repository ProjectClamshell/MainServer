using Npgsql;
using Dapper;

public class Database
{
    private readonly string _connectionString = "Host=postgres;Database=clamshell;Username=postgres;Password=yourpassword";

    public async Task SaveMessageAsync(string content)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.ExecuteAsync(
            "INSERT INTO messages (content, received_at) VALUES (@content, @now)",
            new { content, now = DateTime.UtcNow }
        );
    }

    public async Task<IEnumerable<Message>> GetNewMessagesAsync()
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QueryAsync<Message>("SELECT * FROM messages WHERE received_at >= NOW() - INTERVAL '5 minutes';");
    }

    public async Task<IEnumerable<int>> GetTotalMessagesAsync()
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QueryAsync<int>("SELECT count(*) FROM messages");
    }

    public async Task<IEnumerable<int>> GetSignedMessagesAsync()
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QueryAsync<int>("SELECT count(*) FROM messages where signed=True");
    }

    public async Task<IEnumerable<int>> GetUnSignedMessagesAsync()
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QueryAsync<int>("SELECT count(*) FROM messages where signed=False");
    }

    static string DecryptMessage()
    {
        return string.Empty; // implement later
    } 
}