using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmployeeManagementSystem.Controllers 
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly PasswordHasher<AppUser> _passwordHasher;

        public AuthController(
            ApplicationDbContext context,
            IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _passwordHasher = new PasswordHasher<AppUser>();
        }
        //---- For Hard Code User Name and Password Passing
        //[AllowAnonymous]
        //[HttpPost("login")]
        //public IActionResult Login(LoginRequest request)
        //{
        //    // Temporary user for learning/testing
        //    if (request.Username != "admin" ||
        //        request.Password != "12345")
        //    {
        //        return Unauthorized("Invalid username or password.");
        //    }

        //    var claims = new[]
        //    {
        //        new Claim(ClaimTypes.Name, request.Username) //JWT ke andar user-related information/claims add karta hai.
        //    };

        //    var key = new SymmetricSecurityKey(
        //        Encoding.UTF8.GetBytes(
        //            "EmployeeManagementSystemSecretKey2026@Secure"));

        //    var credentials = new SigningCredentials(
        //        key,
        //        SecurityAlgorithms.HmacSha256);

        //    var token = new JwtSecurityToken(
        //        issuer: "EmployeeManagementSystem", //  → Token kisne issue kiya
        //        audience: "EmployeeManagementSystemUsers", // → Token kiske liye hai 
        //        claims: claims, //  → User information 
        //        expires: DateTime.UtcNow.AddHours(1), //→ Token kab expire hoga
        //        signingCredentials: credentials); //→ Token kis key se sign hoga

        //    var jwtToken = new JwtSecurityTokenHandler()
        //        .WriteToken(token); // JWT object ko actual token string me convert karta hai. 

        //    return Ok(new
        //    {
        //        token = jwtToken  // Client ko token mil jayega
        //    });
        //}

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Username == request.Username);

            if (user == null || !user.IsActive)
            {
                return Unauthorized("Invalid username or password.");
            }

            var passwordResult = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                request.Password);

            if (passwordResult == PasswordVerificationResult.Failed)
            {
                return Unauthorized("Invalid username or password.");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _configuration["Jwt:Key"]!));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials);

            var jwtToken = new JwtSecurityTokenHandler()
                .WriteToken(token);

            return Ok(new
            {
                token = jwtToken
            });
        }
    }
}