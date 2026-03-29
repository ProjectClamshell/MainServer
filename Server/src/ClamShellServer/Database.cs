using Npgsql;
using Dapper;

public interface DatabaseConnection
{
    Task<IEnumerable<Message>> GetNewMessagesAsync(int requestedTime);
    Task<IEnumerable<Message>> GetAllMessagesAsync();
    Task<IEnumerable<Message>> GetMessageByPGN(string requestedPGN);
    Task<IEnumerable<Message>> GetMessageByTimePGN(int requestedTime, string requestedPGN);
    Task<IEnumerable<int>> GetTotalMessagesAsync();
    Task<IEnumerable<Message>> GetSignedMessagesAsync();
    Task<IEnumerable<Message>> GetUnSignedMessagesAsync();
    Task<IEnumerable<Message>> GetValidatedMessagesAsync();
    Task<IEnumerable<Message>> GetUnValidatedMessagesAsync();
    Task<int> ResetTableAsync();
    Task SaveMessageAsync(string content, bool signed, bool validated, string pgn);}
public class Database : DatabaseConnection
{  
    private readonly string _connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__PostgreSQL") ?? "Host=postgres;Database=clamshell;Username=postgres;Password=yourpassword"; //defualt database creds
     
    public async Task<IEnumerable<Message>> GetAllMessagesAsync()
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QueryAsync<Message>("SELECT * FROM messages");
    }

    public async Task<IEnumerable<Message>> GetNewMessagesAsync(int requestedTime)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QueryAsync<Message>("SELECT * FROM messages WHERE receivedAt >= NOW() - (INTERVAL '1 second' * @Time);", new { Time = requestedTime });
    }

    public async Task<IEnumerable<Message>> GetMessageByPGN(string requestedPGN)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QueryAsync<Message>("SELECT * FROM messages WHERE pgn = @RequestedPGN;", new {RequestedPGN = requestedPGN});
    }

    public async Task<IEnumerable<Message>> GetMessageByTimePGN(int requestedTime, string requestedPGN)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QueryAsync<Message>("SELECT * FROM messages WHERE pgn = @RequestedPGN and receivedAt >= NOW() - (INTERVAL '1 second' * @Time);", new {RequestedPGN = requestedPGN, Time = requestedTime});
    }

    public async Task<IEnumerable<int>> GetTotalMessagesAsync()
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QueryAsync<int>("SELECT count(*) FROM messages");
    }

    public async Task<IEnumerable<Message>> GetSignedMessagesAsync()
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QueryAsync<Message>("SELECT * FROM messages where signed = True");
    }

    public async Task<IEnumerable<Message>> GetUnSignedMessagesAsync()
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QueryAsync<Message>("SELECT * FROM messages where signed = False");
    }

    public async Task<IEnumerable<Message>> GetValidatedMessagesAsync()
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QueryAsync<Message>("SELECT * FROM messages where validated = True");
    }

    public async Task<IEnumerable<Message>> GetUnValidatedMessagesAsync()
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QueryAsync<Message>("SELECT * FROM messages where validated = False");
    }

    public async Task<int> ResetTableAsync()
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.ExecuteAsync("TRUNCATE TABLE messages;");
        return 0;
    }

    public async Task SaveMessageAsync(string content, bool signed, bool validated, string pgn)
    {
        var now = DateTime.UtcNow; //current timestamp
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.ExecuteAsync("INSERT INTO messages (content, signed, validated, receivedAt, pgn) VALUES (@content, @signed, @validated, @receivedAt, @pgn)", new { content, signed, validated, receivedAt = now, pgn});
    }
}