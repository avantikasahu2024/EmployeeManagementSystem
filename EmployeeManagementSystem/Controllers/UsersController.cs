using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace EmployeeManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordHasher<AppUser> _passwordHasher;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<AppUser>();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(AppUserRequest request)
        {
            var existingUser = _context.Users
                .FirstOrDefault(u => u.Username == request.Username);

            if (existingUser != null)
            {
                return BadRequest("Username already exists.");
            }

            var user = new AppUser
            {
                Username = request.Username,
                Role = request.Role,
                IsActive = true
            };

            user.PasswordHash =
                _passwordHasher.HashPassword(user, request.Password);

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return Ok("User created successfully.");
        }
    }

    public class AppUserRequest
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }
    }
}