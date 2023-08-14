using Microsoft.AspNetCore.SignalR;
using Minimal_Chat_Application.DataAccessLayer.Models;
using Minimal_Chat_Application.ParameterModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Minimal_Chat_Application.Hubs
{

    public class ChatHub :Hub
    {
        private readonly static ConnectionMapping<string> _connections=new ConnectionMapping<string>();
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ChatHub(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        public override Task OnConnectedAsync()
        {
            var connectionId = Context.ConnectionId;
            var userId = GetUserId();

            _connections.Add(userId,connectionId);

           
           return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var userId = GetUserId();
            var connectionId = Context.ConnectionId;

            _connections.Remove(userId,connectionId);
            
            return base.OnDisconnectedAsync(exception);
        }
        private string GetUserId()
        {
            var query = Context.GetHttpContext().Request.Query;
            var token = query["access_token"];

            if (string.IsNullOrEmpty(token))
            {
                throw new InvalidOperationException("Missing access_token in query string");
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                throw new InvalidOperationException("User ID claim not found in JWT token");
            }
            var userId=userIdClaim.Value;

            return userId;
        }

        //public async Task SendMsg(Message message)
        //{
        //    foreach (var connectionId in _connections.GetConnections(message.ReceiverID))
        //       {
        //          await Clients.Client(connectionId).SendAsync("BroadCast", message);
        //       }
        //}


    }
}
