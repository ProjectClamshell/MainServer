using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class MessagesController : ControllerBase
{
    private readonly Database _db = new Database();

    [HttpGet("total")]
    public async Task<IActionResult> GetTotal() { }

    [HttpGet("signed")]
    public async Task<IActionResult> GetSigned() { }

    [HttpGet("unsigned")]
    public async Task<IActionResult> GetUnSigned() { }

    [HttpPost]
    public async Task<IActionResult> NewMessage([FromBody] string content) { }
}
