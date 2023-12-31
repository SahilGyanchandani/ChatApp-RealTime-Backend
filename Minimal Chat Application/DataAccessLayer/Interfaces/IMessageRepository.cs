﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Minimal_Chat_Application.DataAccessLayer.Models;

namespace Minimal_Chat_Application.DataAccessLayer.Interfaces
{
    public interface IMessageRepository
    {
        Task<List<Message>> GetConversationHistoryAsync(string userId, string receiverId, DateTime? before, int count, bool isAscending);
        Task<Message> SendMessageAsync(Message message);
        Task<bool> EditMessageAsync(int messageId, string content);
        Task<bool> DeleteMessageAsync(int messageId);
        Task<IEnumerable<Message>> SearchConversationsAsync(string currentUserEmail, string query);
        Task<IdentityUser> FindUserByEmailAsync(string email);
    }


}
