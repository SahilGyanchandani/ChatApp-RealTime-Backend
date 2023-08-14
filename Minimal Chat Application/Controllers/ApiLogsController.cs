using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Minimal_Chat_Application.DataAccessLayer.Data;
using Minimal_Chat_Application.DataAccessLayer.Models;
using System.Security.Claims;

namespace Minimal_Chat_Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiLogsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ApiLogsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ApiLog>>> GetApiLogs()
        {
            var logs = await _context.ApiLogs.ToListAsync();
            return Ok(logs);
        }

    }
}
