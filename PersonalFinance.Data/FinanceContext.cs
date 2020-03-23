using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PersonalFinance.Domain;
using PersonalFinance.Domain.Configuration;

namespace PersonalFinance.Data
{
    public class FinanceContext : IdentityDbContext<User>
    {
        public FinanceContext(DbContextOptions<FinanceContext> options)
            : base(options)
        {

        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerBalance> UserBalance { get; set; }
        public DbSet<BalanceHistory> BalanceHistories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        /*Adds a logger to the console*/
        public static readonly ILoggerFactory ConsoleLoggerFactory
            = LoggerFactory.Create(builder =>
            {
                builder.AddFilter((category, level) =>
                category == DbLoggerCategory.Database.Command.Name &&
                level == LogLevel.Information).AddConsole();
            });



        //The method to add predefined configuration options
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new RoleConfiguration());
        }
        /*
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(ConsoleLoggerFactory);
        }
        */

    }
}
