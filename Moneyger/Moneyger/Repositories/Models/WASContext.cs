using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Moneyger.Repositories.Models
{
    public partial class WASContext : DbContext
    {
        public virtual DbSet<CategoryDAO> Category { get; set; }
        public virtual DbSet<TransactionDAO> Transaction { get; set; }
        public virtual DbSet<UserDAO> User { get; set; }
        public virtual DbSet<WalletDAO> Wallet { get; set; }

        public WASContext(DbContextOptions<WASContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseNpgsql("Host=localhost;Database=WAS;Username=postgres;Password=12345678");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Npgsql:PostgresExtension:uuid-ossp", ",,1.1")
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<CategoryDAO>(entity =>
            {
                entity.HasIndex(e => e.CX)
                    .HasName("category_cx_idx")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CX).ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<TransactionDAO>(entity =>
            {
                entity.HasIndex(e => e.CX)
                    .HasName("transaction_cx_idx")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Amount).HasColumnType("numeric");

                entity.Property(e => e.CX).ValueGeneratedOnAdd();

                entity.Property(e => e.Note).HasMaxLength(500);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("transaction_fk");

                entity.HasOne(d => d.Wallet)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.WalletId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("transaction_fk_1");
            });

            modelBuilder.Entity<UserDAO>(entity =>
            {
                entity.HasIndex(e => e.CX)
                    .HasName("user_cx_idx")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CX).ValueGeneratedOnAdd();

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<WalletDAO>(entity =>
            {
                entity.HasIndex(e => e.CX)
                    .HasName("wallet_cx_idx")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Balance).HasColumnType("numeric");

                entity.Property(e => e.CX).ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Wallets)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("wallet_fk");
            });

            OnModelCreatingExt(modelBuilder);
        }

        partial void OnModelCreatingExt(ModelBuilder modelBuilder);
    }
}
