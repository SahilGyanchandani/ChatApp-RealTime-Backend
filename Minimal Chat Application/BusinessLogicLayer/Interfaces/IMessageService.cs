using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Minimal_Chat_Application.ParameterModels;
using Minimal_Chat_Application.Response_Models;

namespace Minimal_Chat_Application.BusinessLogicLayer.Interfaces
{
    public interface IMessageService
    {
        Task<IEnumerable<MessageResponse>> GetConversationHistory(string userId, DateTime? before, int count, string sort, string currentUserEmail);
        Task<MessageResponse> SendMessage(ReceiveMessage msg, string currentUserEmail);
        Task EditMessage(int msgId, EditMessage edit, string currentUserEmail);
        Task DeleteMessage(int msgId, string currentUserEmail);
        Task<IEnumerable<MessageResponse>> SearchConversations(string query, string currentUserEmail);
    }
}
