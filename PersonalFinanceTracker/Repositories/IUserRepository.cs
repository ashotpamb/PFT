using PersonalFinanceTracker.Models;

namespace PersonalFinanceTracker.Repositories;

public interface IUserRepository
{
    Task RegisterUser(UserRegister? userRegister);
    Task<string> LoginUser(UserLogin userLogin);
    bool CheckTokenExpire(string? token);
    string GetClaimFromToken(string token);
    Task<User?> GetUserByEmail(string email);
    Task<List<Transactions>> GetTransactions(int userID);
    Task<bool> AddTransactionToUser(string email, Transactions transactions);
    Task<List<Transactions>> FilterTransactions(int userId, TransactionTypes transactionTypes);
    Task<List<Transactions>> SearchTransactions(int userId, string searchQuery);
}