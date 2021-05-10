using System.Threading;
using System.Threading.Tasks;
using Checkout.PaymentGateway.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Checkout.PaymentGateway.Data
{
    public interface IAppDbContext
    {
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<CardDetails> Cards { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
