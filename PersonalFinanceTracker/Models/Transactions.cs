using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceTracker.Models;

public class Transactions
{
    [Key]
    public int TransactionId { get; set; }
    public decimal TransactionAmount { get; set; }
    public DateTime TransactionDate { get; set; }
    
    public User? User { get; set; }
}