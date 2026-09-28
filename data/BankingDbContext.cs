using Microsoft.EntityFrameworkCore;
using MiniBanking.API.Models;

namespace MiniBanking.API.Data;

public class BankingDbContext : DbContext
{
    public BankingDbContext(DbContextOptions<BankingDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<BankAccount> BankAccounts { get; set; }

    public DbSet<Transaction> Transactions { get; set; }
}