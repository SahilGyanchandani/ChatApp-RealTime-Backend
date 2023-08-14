using Microsoft.AspNetCore.Identity;
using Minimal_Chat_Application.ParameterModels;

namespace Minimal_Chat_Application.BusinessLogicLayer.Interfaces
{
    public interface IUserService
    {
          Task<IdentityUser> AuthenticateGoogleUserAsync(ExternalAuthDto request);
    }
}
