using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YesPojiQuota.Core.Models;

namespace YesPojiQuota.Core.Data
{
    public class YesContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Quota> Quotas { get; set; }
        //public DbSet<App> App { get; set; }
        //public DbSet<Metadata> Metadatas { get; set; }

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
}
