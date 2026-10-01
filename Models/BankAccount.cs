using System.ComponentModel.DataAnnotations;

namespace MiniBanking.API.Models;

public class BankAccount
{
    public int BankAccountId { get; set; }

    [Required]
    public int CustomerId { get; set; }

    [Required]
    public string AccountNumber { get; set; } = "";

    [Required]
    public string AccountType { get; set; } = "";

    [Range(0, double.MaxValue)]
    public decimal Balance { get; set; }

    public Customer? Customer { get; set; }
}