using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PersonalFinanceTracker.Data;
using PersonalFinanceTracker.Models;
using PersonalFinanceTracker.Services;

namespace PersonalFinanceTracker.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DataContext _dataContext;
    private readonly IJwtService _jwtService;

    public UserRepository(DataContext dataContext, IJwtService jwtService)
    {
        _dataContext = dataContext;
        _jwtService = jwtService;
    }

    public async Task RegisterUser(UserRegister? userRegister)
    {
        if (userRegister == null)
            throw new ArgumentNullException(nameof(userRegister), "User registration data is null.");
        if (PasswordNotEmpty(userRegister.Password))
        {
            var passwordHasher = new PasswordHasher<string>();
            var hashedPassword = passwordHasher.HashPassword(null, userRegister.Password);
            var user = new User
            {
                FullName = userRegister.FullName,
                Email = userRegister.Email,
                Password = hashedPassword
            };
            await _dataContext.User.AddAsync(user);
            await _dataContext.SaveChangesAsync();
        }
        else
        {
            throw new ArgumentException("Password is null", nameof(userRegister));
        }
    }

    public async Task<string> LoginUser(UserLogin? userLogin)
    {
        if (userLogin == null) throw new ArgumentNullException(nameof(userLogin), "User params not set");

        if (EmailNotEmpty(userLogin.Email) && PasswordNotEmpty(userLogin.Password))
        {
            var user = await _dataContext.User.FirstOrDefaultAsync(u => u.Email == userLogin.Email);

            if (user == null) throw new Exception($"User with Emil: {userLogin.Email} not found");

            var passwordHasher = new PasswordHasher<string>();
            var verificationResult = passwordHasher.VerifyHashedPassword(null,  user.Password,userLogin.Password);
            if (verificationResult == PasswordVerificationResult.Success)
            {
                var token = _jwtService.GenerateToken(userLogin);
                return token;
            }

            throw new Exception("Wrong Password");
        }
        throw new Exception("Email or password is empty");
    }
    public bool CheckTokenExpire(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        if (tokenHandler.CanReadToken(token))
        {
            var tokenObj = tokenHandler.ReadToken(token) as JwtSecurityToken;

            if (tokenObj != null)
            {
                if (tokenObj.ValidTo > DateTime.UtcNow) return true;
            }
        }

        return false;
    }
    private bool PasswordNotEmpty(string password)
    {
        return !string.IsNullOrEmpty(password);
    }

    private bool EmailNotEmpty(string email)
    {
        return !string.IsNullOrEmpty(email);
    }
}