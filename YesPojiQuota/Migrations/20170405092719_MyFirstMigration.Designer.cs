using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using YesPojiQuota.Data;

namespace YesPojiQuota.Migrations
{
    [DbContext(typeof(YesContext))]
    [Migration("20170405092719_MyFirstMigration")]
    partial class MyFirstMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1");

            modelBuilder.Entity("test2.Data.Account", b =>
                {
                    b.Property<int>("AccountId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Password");

                    b.Property<string>("Username");

                    b.HasKey("AccountId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("test2.Data.Quota", b =>
                {
                    b.Property<int>("QuotaId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccountId");

                    b.Property<decimal>("Available");

                    b.HasKey("QuotaId");

                    b.HasIndex("AccountId")
                        .IsUnique();

                    b.ToTable("Quotas");
                });

            modelBuilder.Entity("test2.Data.Quota", b =>
                {
                    b.HasOne("test2.Data.Account", "Account")
                        .WithOne("Quota")
                        .HasForeignKey("test2.Data.Quota", "AccountId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
