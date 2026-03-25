using Microsoft.AspNetCore.Mvc;

public interface IMessagesApi
{
    Task<IActionResult> GetNew();
    Task<IActionResult> GetTotal();
    Task<IActionResult> GetAll();
    Task<IActionResult> GetSigned();
    Task<IActionResult> GetUnSigned();
    Task<IActionResult> ResetTable();
}

[ApiController]
[Route("api/[controller]")]
public class MessagesController : ControllerBase, IMessagesApi
{
    private readonly Database _db = new Database();

    [HttpGet("new/{pgn}")]
    public async Task<IActionResult> GetNew(int pgn)
    {
        var new_messages = await _db.GetNewMessagesAsync(pgn);
        return Ok(new_messages);
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
    
    [HttpGet("by-pgn/{pgn}")]
    public async Task<IActionResult> GetMessageByPGN(string pgn)
    {
        var messages = await _db.GetMessageByPGN(pgn);
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
