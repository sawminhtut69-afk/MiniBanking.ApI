namespace MiniBanking.API.Models;

public class Transaction
{
    public int TransactionId { get; set; }

    public int BankAccountId { get; set; }

    public string TransactionType { get; set; } = "";

    public decimal Amount { get; set; }

    public DateTime TransactionDate { get; set; }

    public string Description { get; set; } = "";

    public BankAccount? BankAccount { get; set; }
}