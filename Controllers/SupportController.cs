using BreathingFree.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Đảm bảo người dùng đã đăng nhập
public class SupportController : ControllerBase
{
    private readonly SupportService _supportService;

    public SupportController(SupportService supportService)
    {
        _supportService = supportService;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] SendMessageModel model)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var message = await _supportService.SendMessageAsync(userId, model.StaffId, model.Content);
        return Ok(message);
    }

    [HttpGet("conversation/{staffId}")]
    public async Task<IActionResult> GetConversation(int staffId)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var messages = await _supportService.GetConversationAsync(userId, staffId);
        return Ok(messages);
    }
}