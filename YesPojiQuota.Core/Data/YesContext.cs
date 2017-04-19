using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YesPojiQuota.Core.Data
{
    public class YesContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Quota> Quotas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=data.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Make Id required.
            modelBuilder.Entity<Account>()
                .Property(p => p.AccountId)
                .IsRequired();

            modelBuilder.Entity<Quota>()
                .Property(p => p.QuotaId)
                .IsRequired();

        }
    }

    public class Account
    {
        public int AccountId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public virtual Quota Quota { get; set; }

        public Account()
        {

        }

        public Account(string u, string p="")
        {
            Username = u;
            Password = p;
        }
    }

    public class Quota
    {
        public int QuotaId { get; set; }
        public decimal Available { get; set; }

        public int AccountId { get; set; }
        public virtual Account Account { get; set; }
    }
}
