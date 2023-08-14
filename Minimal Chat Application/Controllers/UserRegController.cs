using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Minimal_Chat_Application.DataAccessLayer;
using Minimal_Chat_Application.ParameterModels; 
using Minimal_Chat_Application.Response_Models;
using Minimal_Chat_Application.DataAccessLayer.Interfaces;
using Minimal_Chat_Application.DataAccessLayer.Models;
using System.Security.Claims;
using Minimal_Chat_Application.BusinessLogicLayer.Services;

namespace Minimal_Chat_Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRegController : ControllerBase
    {
        private readonly IUserRegistrationRepository _userRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public UserRegController(IUserRegistrationRepository userRepository, UserManager<IdentityUser> userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }

        [HttpGet("Users")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<IdentityUser>>> GetUser()
        {
            // Get the email of the currently authenticated user
            var currentUserEmail = GetCurrentEmail();

            if (currentUserEmail == null)
            {
                // If the email is null, return an error
                return BadRequest("Unable to retrieve the email of the current user.");
            }

            var users = await _userRepository.GetAllUsersAsync(currentUserEmail);
            if (users == null)
            {
                return NotFound();
            }
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IdentityUser>> GetUserById(string id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost("Register")]
        public async Task<ActionResult> AddUser([FromBody] UserReg reg)
        {
            var userExist = await _userRepository.FindUserByEmailAsync(reg.Email);
            if (userExist != null)
            {
                return Conflict(new { error = "Email is Already Registered" });
            }

            var user = new IdentityUser
            {
                UserName = reg.Name,
                Email = reg.Email,
                //PasswordHash = PasswordHash.HashPassword(reg.Password),
                PhoneNumber = reg.PhoneNumber,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var isUserAdded = await _userRepository.AddUserAsync(user, reg.Password);

            if (isUserAdded)
            {
                var response = new UserAddResponse
                {
                    Name = user.UserName,
                    Email = user.Email
                };
                return Ok(response);
            }
            else
            {
                return BadRequest(new { error = "User failed to create" });
            }
        }
        private string GetCurrentEmail()
        {
            // Get the current user's claims principal
            var claimsPrincipal = HttpContext.User;

            // Get the email claim
            var emailClaim = claimsPrincipal.FindFirst(ClaimTypes.Email);

            return emailClaim?.Value;
        }
    }
}


