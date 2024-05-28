using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace PaymentAPI.Models
{
    public class PaymentContext : DbContext
    {
        public PaymentContext(DbContextOptions<PaymentContext> options) : base(options)
        {
        }

        public DbSet<Payment> Payments { get; set; } = null!;
    }
}
