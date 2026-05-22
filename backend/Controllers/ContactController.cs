using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.DTOs;
using backend.Models;

namespace backend.Controllers;

/// <summary>
/// 联系控制器 - 处理联系表单提交和管理
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ContactController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ContactController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// 提交联系表单 - 公开接口
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> SubmitContact(ContactDto dto)
    {
        var message = new ContactMessage
        {
            Name = dto.Name,
            Email = dto.Email,
            Subject = dto.Subject,
            Message = dto.Message,
            CreatedAt = DateTime.UtcNow
        };

        _context.ContactMessages.Add(message);
        await _context.SaveChangesAsync();

        return Ok(new { message = "消息已发送，感谢您的联系！" });
    }

    /// <summary>
    /// 获取所有联系消息 - 需要管理员权限
    /// </summary>
    [HttpGet("messages")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<List<ContactMessageDto>>> GetMessages()
    {
        var messages = await _context.ContactMessages
            .OrderByDescending(m => m.CreatedAt)
            .Select(m => new ContactMessageDto
            {
                Id = m.Id,
                Name = m.Name,
                Email = m.Email,
                Subject = m.Subject,
                Message = m.Message,
                IsRead = m.IsRead,
                CreatedAt = m.CreatedAt
            })
            .ToListAsync();

        return Ok(messages);
    }

    /// <summary>
    /// 标记消息为已读 - 需要管理员权限
    /// </summary>
    [HttpPut("messages/{id}/read")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        var message = await _context.ContactMessages.FindAsync(id);

        if (message == null)
        {
            return NotFound(new { message = "消息不存在" });
        }

        message.IsRead = true;
        await _context.SaveChangesAsync();

        return Ok(new { message = "已标记为已读" });
    }

    /// <summary>
    /// 删除联系消息 - 需要管理员权限
    /// </summary>
    [HttpDelete("messages/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteMessage(int id)
    {
        var message = await _context.ContactMessages.FindAsync(id);

        if (message == null)
        {
            return NotFound(new { message = "消息不存在" });
        }

        _context.ContactMessages.Remove(message);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
