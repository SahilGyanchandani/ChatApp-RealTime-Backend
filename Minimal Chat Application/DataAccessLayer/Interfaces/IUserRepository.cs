
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Minimal_Chat_Application.DataAccessLayer.Models;


namespace Minimal_Chat_Application.DataAccessLayer.Interfaces
{
    public interface IUserRepository
    {
        Task<IdentityUser> FindUserByEmailAsync(string email);
        Task<bool> CheckPasswordAsync(IdentityUser user, string password);
    }
}
