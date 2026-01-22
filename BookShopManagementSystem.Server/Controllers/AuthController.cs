using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookShop.Server.Models;

namespace BookShop.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly BookShopAPIContext _context;

        public AuthController(BookShopAPIContext context)
        {
            _context = context;
        }

        // POST: api/Auth/login
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { message = "Username and password are required" });
            }

            var user = await _context.User
                .FirstOrDefaultAsync(u => u.Username == request.Username && u.Password == request.Password);

            if (user == null)
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }

            return Ok(new LoginResponse
            {
                UserID = user.UserID,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                Message = "Login successful"
            });
        }

        // POST: api/Auth/register
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(RegisterRequest request)
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { message = "Username and password are required" });
            }

            // Check if user already exists
            var existingUser = await _context.User
                .FirstOrDefaultAsync(u => u.Username == request.Username || u.Email == request.Email);

            if (existingUser != null)
            {
                return Conflict(new { message = "User already exists" });
            }

            var user = new User
            {
                Username = request.Username,
                Password = request.Password, // In production, hash this
                Email = request.Email,
                Role = "User", // Default role
                CreatedAt = DateTime.Now
            };

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Login), new { id = user.UserID }, new
            {
                user.UserID,
                user.Username,
                user.Email,
                user.Role,
                message = "Registration successful"
            });
        }
    }

    // Request/Response models
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class RegisterRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }

    public class LoginResponse
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string? Role { get; set; }
        public string Message { get; set; }
    }
}