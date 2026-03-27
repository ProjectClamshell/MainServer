using Microsoft.AspNetCore.Mvc;

public interface IMessagesApi
{
    Task<IActionResult> GetMessageByPGN(string login, string password);
    Task<IActionResult> GetAll();
    Task<IActionResult> GetTotal();
    Task<IActionResult> GetNew(int time);
    Task<IActionResult> GetMessageByPGN(string pgn);
    Task<IActionResult> GetMessageByTimePGN(string pgn, int time);
    Task<IActionResult> GetSigned();
    Task<IActionResult> GetUnSigned();
    Task<IActionResult> ResetTable();
}

[ApiController]
[Route("api/[controller]")]
public class MessagesController : ControllerBase, IMessagesApi
{
    private readonly Database _db = new Database();
    private readonly string defaultUsername = Environment.GetEnvironmentVariable("DEFAULTUSERNAME") ?? throw new InvalidOperationException("DEFAULTUSERNAME not set");
    private readonly string defaultPassword = Environment.GetEnvironmentVariable("DEFAULTPASSWORD") ?? throw new InvalidOperationException("DEFAULTPASSWORD not set");

    [HttpGet("login/{username}/{password}")]
    public async Task<IActionResult> GetMessageByPGN(string username, string password)
    {
        bool validLogin;
        if (defaultUsername == username && defaultPassword == password){validLogin = true;} else {validLogin = false;}
        return Ok(validLogin);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        var new_messages = await _db.GetAllMessagesAsync();
        return Ok(new_messages);
    }

    [HttpGet("total")]
    public async Task<IActionResult> GetTotal()
    {
        var total_messages = await _db.GetTotalMessagesAsync();
        return Ok(total_messages);
    }

    [HttpGet("new/{time}")]
    public async Task<IActionResult> GetNew(int time)
    {
        var new_messages = await _db.GetNewMessagesAsync(time);
        return Ok(new_messages);
    }
    
    [HttpGet("by-pgn/{pgn}")]
    public async Task<IActionResult> GetMessageByPGN(string pgn)
    {
        var messages = await _db.GetMessageByPGN(pgn);
        return Ok(messages);
    }

    [HttpGet("by-pgn/{pgn}/since/{time}")]
    public async Task<IActionResult> GetMessageByTimePGN(string pgn, int time)
    {
        var messages = await _db.GetMessageByTimePGN(time, pgn);
        return Ok(messages);
    }

    [HttpGet("signed")]
    public async Task<IActionResult> GetSigned()
    {
        var signed_messages = await _db.GetSignedMessagesAsync();
        return Ok(signed_messages);
    }

    [HttpGet("unsigned")]
    public async Task<IActionResult> GetUnSigned()
    {
        var unsigned_messages = await _db.GetUnSignedMessagesAsync();
        return Ok(unsigned_messages);
    }

    [HttpGet("Reset")] //for testing only
    public async Task<IActionResult> ResetTable()
    {
        var reset_table = await _db.ResetTableAsync();
        return Ok(reset_table);
    }
}
