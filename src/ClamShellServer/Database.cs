using Npgsql;
using Dapper;
using System.Text;
using System.Runtime.CompilerServices;

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

    public async Task<IEnumerable<Message>> GetTotalMessagesAsync()
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        return await connection.QueryAsync<Message>("SELECT * FROM messages");
    }

    public async Task<IEnumerable<Message>> GetSignedMessagesAsync()
    {
    }

    public async Task<IEnumerable<Message>> GetUnSignedMessagesAsync()
    {
    }

    static string DecryptMessage()
    {
    }    
}