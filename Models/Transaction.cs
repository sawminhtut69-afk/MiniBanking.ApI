using System.ComponentModel.DataAnnotations;

namespace MiniBanking.API.Models;

public class Transaction
{
    public int TransactionId { get; set; }

    [Required]
    public int BankAccountId { get; set; }

    [Required]
    public string TransactionType { get; set; } = "";

    [Range(0.01, double.MaxValue)]
    public decimal Amount { get; set; }

    public DateTime TransactionDate { get; set; }

    [StringLength(200)]
    public string Description { get; set; } = "";

    public BankAccount? BankAccount { get; set; }
}