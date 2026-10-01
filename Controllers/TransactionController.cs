using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniBanking.API.Data;
using MiniBanking.API.Models;

namespace MiniBanking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TransactionController : ControllerBase
{
    private readonly BankingDbContext _context;

    public TransactionController(BankingDbContext context)
    {
        _context = context;
    }

    // Transaction History
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions()
    {
        var username = User.Identity?.Name;

        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Username == username);

        if (user == null)
        {
            return Unauthorized();
        }

        if (user.Role == "Admin")
        {
            return await _context.Transactions
                .ToListAsync();
        }

        if (user.CustomerId == null)
        {
            return BadRequest("Customer account is not linked.");
        }

        var transactions = await _context.Transactions
            .Where(t => t.BankAccount != null &&
                        t.BankAccount.CustomerId == user.CustomerId)
            .ToListAsync();

        return transactions;
    }

    // Deposit
    [HttpPost("deposit")]
    public async Task<ActionResult<Transaction>> Deposit(
        int bankAccountId,
        decimal amount)
    {
        var username = User.Identity?.Name;

        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Username == username);

        if (user == null)
        {
            return Unauthorized();
        }

        var account = await _context.BankAccounts
            .FindAsync(bankAccountId);

        if (account == null)
        {
            return NotFound("Bank account not found.");
        }

        if (user.Role != "Admin" &&
            user.CustomerId != account.CustomerId)
        {
            return Forbid();
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

    // Withdraw
    [HttpPost("withdraw")]
    public async Task<ActionResult<Transaction>> Withdraw(
        int bankAccountId,
        decimal amount)
    {
        var username = User.Identity?.Name;

        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Username == username);

        if (user == null)
        {
            return Unauthorized();
        }

        var account = await _context.BankAccounts
            .FindAsync(bankAccountId);

        if (account == null)
        {
            return NotFound("Bank account not found.");
        }

        if (user.Role != "Admin" &&
            user.CustomerId != account.CustomerId)
        {
            return Forbid();
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
    // Transfer
    [HttpPost("transfer")]
    public async Task<ActionResult<Transaction>> Transfer(
        int fromAccountId,
        int toAccountId,
        decimal amount)
    {
        var username = User.Identity?.Name;

        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Username == username);

        if (user == null)
        {
            return Unauthorized();
        }

        if (fromAccountId == toAccountId)
        {
            return BadRequest(
                "Source and destination accounts must be different.");
        }

        var fromAccount = await _context.BankAccounts
            .FindAsync(fromAccountId);

        var toAccount = await _context.BankAccounts
            .FindAsync(toAccountId);

        if (fromAccount == null || toAccount == null)
        {
            return NotFound("Bank account not found.");
        }

        if (user.Role != "Admin" &&
            user.CustomerId != fromAccount.CustomerId)
        {
            return Forbid();
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