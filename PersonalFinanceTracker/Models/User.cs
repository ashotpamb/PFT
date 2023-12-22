using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceTracker.Models;

public class User
{
    [Key] public long UserId { get; set; }

    [Required] public string? FullName { get; set; }

    [Required] [EmailAddress] public string? Email { get; set; }

    [Required] [PasswordPropertyText] public string? Password { get; set; }
    
    public decimal Balance { get; set; }
    
    public ICollection<Transactions>? Transactions { get; set; }
}