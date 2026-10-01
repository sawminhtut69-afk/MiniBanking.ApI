using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MiniBanking.API.Data;
using MiniBanking.API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MiniBanking.API.Controllers;

[ApiController]
[Route("api/Auth")]
public class AuthController : ControllerBase
{
    private readonly BankingDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthController(
        BankingDbContext context,
        IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpPost("/api/Auth/register")]
    public async Task<IActionResult> Register(User user)
    {
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(x => x.Username == user.Username);

        if (existingUser != null)
        {
            return BadRequest("Username already exists.");
        }

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok("Registration successful.");
    }

      [HttpPost("/api/Auth/login")]
       public async Task<IActionResult> Login(User user)
    {
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(x => x.Username == user.Username);

        if (existingUser == null)
        {
            return Unauthorized("Invalid username or password.");
        }

        bool passwordValid = BCrypt.Net.BCrypt.Verify(
            user.PasswordHash,
            existingUser.PasswordHash);

        if (!passwordValid)
        {
            return Unauthorized("Invalid username or password.");
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, existingUser.Username),
            new Claim(ClaimTypes.Role, existingUser.Role)
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
        return Ok(new
        {
            token = new JwtSecurityTokenHandler()
                .WriteToken(token)
        });
    }
}