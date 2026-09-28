using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniBanking.API.Data;
using MiniBanking.API.Models;

namespace MiniBanking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BankAccountController : ControllerBase
{
    private readonly BankingDbContext _context;

    public BankAccountController(BankingDbContext context)
    {
        _context = context;
    }

    // GET: api/BankAccount
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BankAccount>>> GetBankAccounts()
    {
        return await _context.BankAccounts.ToListAsync();
    }

    // GET: api/BankAccount/1
    [HttpGet("{id}")]
    public async Task<ActionResult<BankAccount>> GetBankAccount(int id)
    {
        var account = await _context.BankAccounts.FindAsync(id);

        if (account == null)
        {
            return NotFound();
        }

        return account;
    }

    // POST: api/BankAccount
    [HttpPost]
    public async Task<ActionResult<BankAccount>> CreateBankAccount(
        BankAccount account)
    {
        _context.BankAccounts.Add(account);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetBankAccount),
            new { id = account.BankAccountId },
            account);
    }

    // PUT: api/BankAccount/1
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBankAccount(
        int id,
        BankAccount account)
    {
        if (id != account.BankAccountId)
        {
            return BadRequest();
        }

        _context.Entry(account).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/BankAccount/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBankAccount(int id)
    {
        var account = await _context.BankAccounts.FindAsync(id);

        if (account == null)
        {
            return NotFound();
        }

        _context.BankAccounts.Remove(account);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}