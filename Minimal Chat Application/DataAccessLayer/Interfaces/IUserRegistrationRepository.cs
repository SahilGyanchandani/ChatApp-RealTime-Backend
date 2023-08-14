using Microsoft.AspNetCore.Identity;
using Minimal_Chat_Application.DataAccessLayer.Models;
using Minimal_Chat_Application.ParameterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minimal_Chat_Application.DataAccessLayer.Interfaces
{
     public interface IUserRegistrationRepository
    {
        Task<IEnumerable<IdentityUser>> GetAllUsersAsync(string currentUserEmail);
        Task<IdentityUser> GetUserByIdAsync(string id);
        Task<IdentityUser> FindUserByEmailAsync(string email);
        Task<bool> AddUserAsync(IdentityUser user,string password);

    }
}
