using PersonalFinanceTracker.Models;

namespace PersonalFinanceTracker.Repositories;

public interface IUserRepository
{
    Task RegisterUser(UserRegister? userRegister);
    Task<string> LoginUser(UserLogin userLogin);
    bool CheckTokenExpire(string? token);
    string GetClaimFromToken(string token);
    Task<User?> GetUserByEmail(string email);
}