using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using MyApp.Api.Data;
using MyApp.Api.Models.Users;

namespace MyApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly PasswordHasher<User> _passwordHasher;

    public AuthController(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
        _passwordHasher = new PasswordHasher<User>();
    }

    public class LoginRequest
    {
        public string username { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
    }

    public class RegisterRequest
    {
        public string username { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public IActionResult Register([FromBody] RegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.username) || string.IsNullOrWhiteSpace(request.password))
            return BadRequest(new { message = "Username and password are required." });

        if (_context.Users.Any(u => u.Username == request.username))
            return Conflict(new { message = "User already exists." });

        var user = new User
        {
            Username = request.username
        };

        user.Password = _passwordHasher.HashPassword(user, request.password);

        _context.Users.Add(user);
        _context.SaveChanges();

        return CreatedAtAction(null, new { id = user.Id, username = user.Username });
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        // Find user by username
        var user = _context.Users.SingleOrDefault(u => u.Username == request.username);
        if (user is null)
            return Unauthorized(new { message = "Invalid credentials" });

        // Verify hashed password
        var verification = _passwordHasher.VerifyHashedPassword(user, user.Password, request.password);
        if (verification == PasswordVerificationResult.Failed)
            return Unauthorized(new { message = "Invalid credentials" });

        var key = _configuration["Jwt:Key"] ?? "very_secret_dev_key_please_change";
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenKey = Encoding.UTF8.GetBytes(key);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            }),
            Expires = DateTime.UtcNow.AddHours(4),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwt = tokenHandler.WriteToken(token);

        return Ok(new { jwt });
    }
}