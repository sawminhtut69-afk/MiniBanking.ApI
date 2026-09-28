using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniBanking.API.Data;
using MiniBanking.API.Models;

namespace MiniBanking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionController : ControllerBase
{
    private readonly BankingDbContext _context;

    public TransactionController(BankingDbContext context)
    {
        _context = context;
    }

    // GET: api/Transaction
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions()
    {
        return await _context.Transactions.ToListAsync();
    }

    // POST: api/Transaction/deposit
    [HttpPost("deposit")]
    public async Task<ActionResult<Transaction>> Deposit(
        int bankAccountId,
        decimal amount)
    {
        var account = await _context.BankAccounts.FindAsync(bankAccountId);

        if (account == null)
        {
            return NotFound("Bank account not found.");
        }

        if (amount <= 0)
        {
            return BadRequest("Amount must be greater than 0.");
        }

        account.Balance += amount;

        var transaction = new Transaction
        {
            BankAccountId = bankAccountId,
            TransactionType = "Deposit",
            Amount = amount,
            TransactionDate = DateTime.Now,
            Description = "Cash deposit"
        };

        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();

        return Ok(transaction);
    }

    // POST: api/Transaction/withdraw
    [HttpPost("withdraw")]
    public async Task<ActionResult<Transaction>> Withdraw(
        int bankAccountId,
        decimal amount)
    {
        var account = await _context.BankAccounts.FindAsync(bankAccountId);

        if (account == null)
        {
            return NotFound("Bank account not found.");
        }

        if (amount <= 0)
        {
            return BadRequest("Amount must be greater than 0.");
        }

        if (account.Balance < amount)
        {
            return BadRequest("Insufficient balance.");
        }

        account.Balance -= amount;

        var transaction = new Transaction
        {
            BankAccountId = bankAccountId,
            TransactionType = "Withdraw",
            Amount = amount,
            TransactionDate = DateTime.Now,
            Description = "Cash withdrawal"
        };

        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();

        return Ok(transaction);
    }

// POST: api/Transaction/transfer
[HttpPost("transfer")]
public async Task<IActionResult> Transfer(
    int fromAccountId,
    int toAccountId,
    decimal amount)
{
    var fromAccount = await _context.BankAccounts.FindAsync(fromAccountId);
    var toAccount = await _context.BankAccounts.FindAsync(toAccountId);

    if (fromAccount == null || toAccount == null)
    {
        return NotFound("Bank account not found.");
    }

    if (amount <= 0)
    {
        return BadRequest("Amount must be greater than 0.");
    }

    if (fromAccount.Balance < amount)
    {
        return BadRequest("Insufficient balance.");
    }

    fromAccount.Balance -= amount;
    toAccount.Balance += amount;

    var transaction = new Transaction
    {
        BankAccountId = fromAccountId,
        TransactionType = "Transfer",
        Amount = amount,
        TransactionDate = DateTime.Now,
        Description = $"Transfer to account {toAccountId}"
    };

    _context.Transactions.Add(transaction);

    await _context.SaveChangesAsync();

    return Ok(transaction);
    }    
}