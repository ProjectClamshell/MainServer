using Npgsql;
using Dapper;

public class Database
{
    private readonly string _connectionString = "Host=localhost;Database=clamshell;Username=postgres;Password=yourpassword";

    public async Task SaveMessageAsync(string content)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.ExecuteAsync(
            "INSERT INTO messages (content, received_at) VALUES (@content, @now)",
            new { content, now = DateTime.UtcNow }
        );
    }

    public async Task<IEnumerable<Message>> GetNewMessageAsync()
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QueryAsync<Message>("SELECT * FROM messages WHERE received_at >= NOW() - INTERVAL '5 minutes';");
    }

    public async Task<IEnumerable<Message>> GetTotalMessagesAsync()
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QueryAsync<Message>("SELECT count(*) FROM messages");
    }

    public async Task<IEnumerable<Message>> GetSignedMessagesAsync()
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QueryAsync<Message>("SELECT count(*) FROM messages where signed=True");
    }

    public async Task<IEnumerable<Message>> GetUnSignedMessagesAsync()
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QueryAsync<Message>("SELECT count(*) FROM messages where signed=False");
    }

    static string DecryptMessage()
    {
    }    
}