using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class MessagesController : ControllerBase
{
    private readonly Database _db = new Database();

    [HttpGet("new")]
    public async Task<IActionResult> GetNewMessagesAsync()
    {
        var new_messages = await _db.GetNewMessagesAsync();
        return Ok(new_messages);
    }

    [HttpGet("total")]
    public async Task<IActionResult> GetTotal()
    {
        var total_messages = await _db.GetTotalMessagesAsync();
        return Ok(total_messages);
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

    [HttpPost]
    public async Task<IActionResult> SaveNewMessage([FromBody] string content)
    {
        await _db.SaveMessageAsync(content);
        return Ok("saved");
    }
}
