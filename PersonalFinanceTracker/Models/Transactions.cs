using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceTracker.Models;

public class Transactions
{
    [Key] public int TransactionId { get; set; }

    public decimal TransactionAmount { get; set; }
    public string? TransactionDescription { get; set; }
    public DateTime TransactionDate { get; set; }

    public User? User { get; set; }

    [DefaultValue(TransactionTypes.Expanse)]
    public TransactionTypes TransactionType { get; set; }

    public override string ToString()
    {
        return $"TransactionId: {TransactionId}, " +
               $"TransactionAmount: {TransactionAmount}, " +
               $"TransactionDescription: {TransactionDescription ?? "N/A"}, " +
               $"TransactionDate: {TransactionDate}, " +
               $"TransactionType: {TransactionType}, " +
               $"User: {User?.ToString() ?? "N/A"}";
    }
}