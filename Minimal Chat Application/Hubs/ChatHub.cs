using Microsoft.AspNetCore.SignalR;
using StackExchange.Redis;
using Minimal_Chat_Application.DataAccessLayer.Models;
using Minimal_Chat_Application.ParameterModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Minimal_Chat_Application.Hubs
{

    public class ChatHub : Hub
    {
        //private readonly static ConnectionMapping<string> _connections=new ConnectionMapping<string>();
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly RedisConnection _connection;

        public ChatHub(IHttpContextAccessor httpContextAccessor, RedisConnection connection)
        {
            _httpContextAccessor = httpContextAccessor;
            _connection = connection;
        }

        public override async Task OnConnectedAsync()
        {
            var connectionId = Context.ConnectionId;
            var userId = GetUserId();

            await _connection.AddConnectionAsync(userId, connectionId);

            await base.OnConnectedAsync();
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
            var userId = userIdClaim.Value;

            return userId;
        }
       
    }
}
