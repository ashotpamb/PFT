using Microsoft.AspNetCore.Identity;
using PersonalFinanceTracker.Data;
using PersonalFinanceTracker.Models;

namespace PersonalFinanceTracker.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DataContext _dataContext;

    public UserRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task RegisterUser(UserRegister? userRegister)
    {
        if (userRegister == null)
            throw new ArgumentNullException(nameof(userRegister), "User registration data is null.");
        var passwordHasher = new PasswordHasher<string>();
        if (userRegister.Password != null)
        {
            var hashedPassword = passwordHasher.HashPassword(null, userRegister.Password);
            var user = new User
            {
                FullName = userRegister.FullName,
                Email = userRegister.Email,
                Password = hashedPassword
            };
            await _dataContext.User.AddAsync(user);
        }
        else
        {
            throw new ArgumentException("Password is null", nameof(userRegister));
        }

        await _dataContext.SaveChangesAsync();
    }
}