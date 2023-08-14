using Minimal_Chat_Application.DataAccessLayer.Data;
using Minimal_Chat_Application.DataAccessLayer.Models;
using NuGet.Protocol.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Minimal_Chat_Application.DataAccessLayer.Interfaces;
using Message = Minimal_Chat_Application.DataAccessLayer.Models.Message;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Minimal_Chat_Application.DataAccessLayer.Repository
{
    public class MessageRepository : IMessageRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public MessageRepository(AppDbContext context, UserManager<IdentityUser> userManage)
        {
            _context = context;
            _userManager = userManage;
        }

        public async Task<List<Message>> GetConversationHistoryAsync(string userId, string receiverId, DateTime? before, int count, bool isAscending)
        {
            var messages = await _context.Messages
                .Where(m => m.Id == userId && m.ReceiverID == receiverId || m.Id == receiverId && m.ReceiverID == userId)
                .Where(m => before == null || (isAscending ? m.Timestamp < before : m.Timestamp > before))
                .OrderByDescending(m => m.Timestamp)
                .Take(count)
                .ToListAsync();

            return messages;

        }

        public async Task<Message> SendMessageAsync(Message message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<bool> EditMessageAsync(int messageId, string content)
        {
            var message = await _context.Messages.FindAsync(messageId);
            if (message == null)
                return false;

            message.Content = content;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteMessageAsync(int messageId)
        {
            var message = await _context.Messages.FindAsync(messageId);
            if (message == null)
                return false;

            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Message>> SearchConversationsAsync(string currentUserEmail, string query)
        {
            var currentUser = await FindUserByEmailAsync(currentUserEmail);
            if (currentUser == null)
            {
                return null;
            }

            // Search for messages that contain the provided keywords in the conversation of the current user
            var conversations = await _context.Messages
                .Where(m => (m.Id == currentUser.Id || m.ReceiverID == currentUser.Id) &&
                            EF.Functions.Like(m.Content, $"%{query}%")) // Using EF.Functions.Like for case-insensitive comparison
                .OrderBy(m => m.Timestamp)
                .ToListAsync();

            return conversations;
        }
        public async Task<IdentityUser> FindUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

    }
}
