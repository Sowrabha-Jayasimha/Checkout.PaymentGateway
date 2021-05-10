using Checkout.PaymentGateway.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Checkout.PaymentGateway.Data
{
    public class AppDbContext: DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<CardDetails> Cards { get; set; }
    }
}
