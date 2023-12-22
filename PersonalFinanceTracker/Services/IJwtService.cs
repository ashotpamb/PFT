using System.IdentityModel.Tokens.Jwt;
using PersonalFinanceTracker.Models;

namespace PersonalFinanceTracker.Services;

public interface IJwtService
{
    string GenerateToken(UserLogin userLogin);
}