using LightCap.InvestmentApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace  LightCap.InvestmentApi.Infrastructure.Persistence.DbContexts;

//UNCOMMENT LATER WHEN WE HAVE ENTITIES TO ADD
public class AppDbContext(DbContextOptions<AppDbContext> options): DbContext(options)
    //, IAppDbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserLogin> UserLogins { get; set; }
    public DbSet<Otp> Otps { get; set; }
    public DbSet<LinkedBankAccount> LinkedBankAccounts { get; set; }
    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<WalletTransaction> WalletTransactions { get; set; }





    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        // Apply consistent precision to every decimal property in the model,
        // instead of configuring each one individually.
        foreach (var property in modelBuilder.Model.GetEntityTypes()
                     .SelectMany(t => t.GetProperties())
                     .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
        {
            property.SetPrecision(18);
            property.SetScale(2);
        }
    }
    private static string Normalize(string s) => s.Replace(" ", "").Replace("-", "");
}
