using Npgsql;
using Dapper;

public interface DatabaseConnection
{
    Task<IEnumerable<Message>> GetNewMessagesAsync();
    Task<IEnumerable<Message>> GetAllMessagesAsync();
    Task<IEnumerable<int>> GetTotalMessagesAsync();
    Task<IEnumerable<int>> GetSignedMessagesAsync();
    Task<IEnumerable<int>> GetUnSignedMessagesAsync();
    Task<int> ResetTableAsync();
    Task SaveMessageAsync(string content, bool signed);
}

public class Database : DatabaseConnection
{  
    private readonly string _connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__PostgreSQL") ?? "Host=postgres;Database=clamshell;Username=postgres;Password=yourpassword"; //defualt database creds
        
    public async Task<IEnumerable<Message>> GetAllMessagesAsync()
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QueryAsync<Message>("SELECT * FROM messages");
    }

    public async Task<IEnumerable<Message>> GetNewMessagesAsync()
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QueryAsync<Message>("SELECT * FROM messages WHERE received_at >= NOW() - INTERVAL '5 seconds';");
    }

    public async Task<IEnumerable<int>> GetTotalMessagesAsync()
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QueryAsync<int>("SELECT count(*) FROM messages");
    }

    public async Task<IEnumerable<int>> GetSignedMessagesAsync()
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QueryAsync<int>("SELECT count(*) FROM messages where signed = True");
    }

    public async Task<IEnumerable<int>> GetUnSignedMessagesAsync()
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QueryAsync<int>("SELECT count(*) FROM messages where signed = False");
    }

    public async Task<int> ResetTableAsync()
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.ExecuteAsync("TRUNCATE TABLE messages;");
        return 0;
    }

    public async Task SaveMessageAsync(string content, bool signed)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.ExecuteAsync("INSERT INTO messages (content, received_at, signed) VALUES (@content, @now, @signed)", new { content, now = DateTime.UtcNow, signed});
    }

}