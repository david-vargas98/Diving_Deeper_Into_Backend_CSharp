using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApiEcommerce.Models;
using ApiEcommerce.Models.Dtos;
using ApiEcommerce.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ApiEcommerce.Repository;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _db;
    private readonly string? secretKey;
    public UserRepository(ApplicationDbContext db, IConfiguration configuration)
    {
        _db = db;
        secretKey = configuration.GetValue<string>("ApiSettings:SecretKey");
    }
    public User? GetUser(int id)
    {
        return _db.Users.FirstOrDefault(u => u.Id == id);
    }

    public ICollection<User> GetUsers()
    {
        return _db.Users.OrderBy(u => u.UserName).ToList();
    }

    public bool IsUniqueUser(string username)
    {
        var usernameNormalized = username.ToLower().Trim();
        return !_db.Users.Any(u => u.UserName.ToLower().Trim() == usernameNormalized);
    }

    public async Task<UserLoginResponseDto> Login(UserLoginDto userLoginDto)
    {
        if (string.IsNullOrEmpty(userLoginDto.Username))
            return new UserLoginResponseDto()
            {
                Token = "",
                User = null,
                Message = "The username is required!"
            };

        var usernameNormalized = userLoginDto.Username.ToLower().Trim();
        var user = await _db.Users.FirstOrDefaultAsync<User>(u => u.UserName.ToLower().Trim() == usernameNormalized);

        if (user == null)
            return new UserLoginResponseDto()
            {
                Token = "",
                User = null,
                Message = "The username was not found."
            };

        if (!BCrypt.Net.BCrypt.Verify(userLoginDto.Password, user.Password))
            return new UserLoginResponseDto()
            {
                Token = "",
                User = null,
                Message = "The credentials are not correct."
            };

        // JWT generation 
        var handlerToken = new JwtSecurityTokenHandler();
        if (string.IsNullOrWhiteSpace(secretKey))
            throw new InvalidOperationException("Secret key is not configured.");

        var key = Encoding.UTF8.GetBytes(secretKey); // conversion from string byte[]: cryptographic libraries don't work with strings
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity( // identifies the user without querying the DB over and over again
                new[]
                {
                    new Claim("id", user.Id.ToString()),
                    new Claim("username", user.UserName.ToString()),
                    new Claim(ClaimTypes.Role, user.Role ?? string.Empty)
                }
            ),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = handlerToken.CreateToken(tokenDescriptor);

        return new UserLoginResponseDto()
        {
            Token = handlerToken.WriteToken(token),
            User = new UserRegisterDto()
            {
                UserName = user.UserName,
                Name = user.Name,
                Role = user.Role,
                Password = user.Password ?? ""
            },
            Message = "User was logged in correctly!"
        };
    }

    public async Task<User> Register(CreateUserDto createUserDto)
    {
        var encryptedPassword = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password);

        var user = new User()
        {
            UserName = createUserDto.Username ?? "No Username",
            Name = createUserDto.Name,
            Role = createUserDto.Role,
            Password = encryptedPassword
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return user;
    }
}
