using PersonalFinanceTracker.Models;

namespace PersonalFinanceTracker.Repositories;

public interface IUserRepository
{
    Task RegisterUser(UserRegister? userRegister);
}