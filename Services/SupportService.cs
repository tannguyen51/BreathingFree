using BreathingFree.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class SupportService
{
    private readonly ApplicationDbContext _context;

    public SupportService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Message> SendMessageAsync(int senderId, int staffId, string content)
    {
        // Kiểm tra senderId có tồn tại không
        var senderExists = await _context.Users.AnyAsync(u => u.UserID == senderId);
        if (!senderExists)
            throw new ArgumentException("Người gửi không tồn tại.");

        // Kiểm tra staffId có tồn tại không
        var staffExists = await _context.Users.AnyAsync(u => u.UserID == staffId && u.RoleID == 1);
        if (!staffExists)
            throw new ArgumentException("Nhân viên hỗ trợ không tồn tại.");

        var message = new Message
        {
            FromUserID = senderId,
            ToUserID = staffId,
            MessageContent = content,
            SentAt = DateTime.UtcNow
        };

        _context.Messages.Add(message);
        await _context.SaveChangesAsync();
        return message;
    }

    public async Task<List<Message>> GetConversationAsync(int userId, int staffId)
    {
        return await _context.Messages
            .Where(m => (m.FromUserID == userId && m.ToUserID == staffId) ||
                        (m.FromUserID == staffId && m.ToUserID == userId))
            .OrderBy(m => m.SentAt)
            .ToListAsync();
    }

    // Nếu muốn lấy tin nhắn chưa đọc, cần thêm trường IsRead vào bảng Messages.
    // Nếu không có, bạn có thể bỏ qua hàm này hoặc chỉnh lại cho phù hợp.
    // public async Task<List<Message>> GetUserUnreadMessagesAsync(int userId)
    // {
    //     return await _context.Messages
    //         .Where(m => m.ToUserID == userId && !m.IsRead)
    //         .OrderBy(m => m.SentAt)
    //         .ToListAsync();
    // }

    // public async Task MarkMessageAsReadAsync(int messageId)
    // {
    //     var message = await _context.Messages.FindAsync(messageId);
    //     if (message != null)
    //     {
    //         message.IsRead = true;
    //         await _context.SaveChangesAsync();
    //     }
    // }
}