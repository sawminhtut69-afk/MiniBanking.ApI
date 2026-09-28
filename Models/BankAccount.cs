namespace MiniBanking.API.Models;

public class BankAccount
{
    public int BankAccountId { get; set; }

    public int CustomerId { get; set; }

    public string AccountNumber { get; set; } = "";

    public string AccountType { get; set; } = "";

    public decimal Balance { get; set; }
    public Customer? Customer { get; set; }
}