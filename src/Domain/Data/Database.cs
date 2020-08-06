using System;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Domain.Data
{
    using Entities;
    public class Database : DbContext
    {
        public Database(DbContextOptions<Database> options) : base(options) { }
        public DbSet<Merchant> Merchants { get; set; }
        public DbSet<Payment> Payments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MerchantConfiguration());
            modelBuilder.ApplyConfiguration(new PaymentConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
