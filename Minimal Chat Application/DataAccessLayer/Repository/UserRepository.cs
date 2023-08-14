// DataAccessLayer/UserRepository.cs

using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Minimal_Chat_Application.DataAccessLayer.Data;
using Minimal_Chat_Application.DataAccessLayer.Interfaces;
using Minimal_Chat_Application.DataAccessLayer.Models;


namespace Minimal_Chat_Application.DataAccessLayer.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public UserRepository(AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IdentityUser> FindUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<bool> CheckPasswordAsync(IdentityUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }
    }
}
