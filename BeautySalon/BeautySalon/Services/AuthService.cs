using BeautySalonApi.Contracts;
using BeautySalonApi.Data;
using BeautySalonApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BeautySalonApi.Services;

public class AuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(
        AppDbContext context,
        IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<object> Register(RegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.FirstName) ||
            string.IsNullOrWhiteSpace(request.LastName) ||
            string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password) ||
            string.IsNullOrWhiteSpace(request.Phone))
        {
            throw new ArgumentException(
                "All registration fields are required.");
        }

        var role = request.Role.Trim();

        if (!role.Equals("Client",
                StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException(
                "Public registration is available only for clients.");
        }

        role = "Client";
        var email = request.Email.Trim();

        if (await _context.Users.AnyAsync(u => u.Email == email))
            throw new InvalidOperationException("User already exists.");

        var firstName = request.FirstName.Trim();
        var lastName = request.LastName.Trim();
        var phone = request.Phone.Trim();

        var user = new User
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            PasswordHash =
                BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = role
        };

        _context.Users.Add(user);

        var client = new Client
        {
            User = user,
            FirstName = firstName,
            LastName = lastName,
            Phone = phone
        };

        _context.Clients.Add(client);
        await _context.SaveChangesAsync();

        return new
        {
            token = GenerateToken(user),
            user = new
            {
                user.Id,
                user.Email,
                user.FirstName,
                user.LastName,
                user.Role
            },
            profileId = client.Id
        };
    }

    public async Task<object> CreateAdmin(
        AdminCreateRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.FirstName) ||
            string.IsNullOrWhiteSpace(request.LastName) ||
            string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            throw new ArgumentException("All fields are required.");
        }

        var email = request.Email.Trim();

        if (await _context.Users.AnyAsync(u => u.Email == email))
            throw new InvalidOperationException("User already exists.");

        var user = new User
        {
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Email = email,
            PasswordHash =
                BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = "Admin"
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return new
        {
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName,
            user.Role
        };
    }

    public async Task<object> Login(LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            throw new ArgumentException(
                "Email and password required.");
        }

        var user = await _context.Users.FirstOrDefaultAsync(
            u => u.Email == request.Email.Trim());

        if (user == null)
            throw new UnauthorizedAccessException();

        var valid = BCrypt.Net.BCrypt.Verify(
            request.Password, user.PasswordHash);

        if (!valid)
            throw new UnauthorizedAccessException();

        return new
        {
            token = GenerateToken(user),
            user = new
            {
                user.Id,
                user.Email,
                user.FirstName,
                user.LastName,
                user.Role
            }
        };
    }

    private string GenerateToken(User user)
    {
        var jwtSettings =
            _configuration.GetSection("JwtSettings");

        var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

        var claims = new[]
        {
            new Claim("email", user.Email),
            new Claim(
                ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(
                ClaimTypes.Name,
                $"{user.FirstName} {user.LastName}"),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}
