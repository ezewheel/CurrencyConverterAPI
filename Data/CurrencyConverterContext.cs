using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Data
{
    public class CurrencyConverterContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Currency> Currencies { get; set; }

        public CurrencyConverterContext(DbContextOptions<CurrencyConverterContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.Property(u => u.Username)
                    .IsRequired();

                entity.Property(u => u.Name)
                    .IsRequired();

                entity.Property(u => u.Password)
                    .IsRequired();

                entity.Property(u => u.SubscribedUntil);

                entity.Property(u => u.ConversionsCount)
                    .IsRequired();

                entity.Property(u => u.isDeleted)
                    .IsRequired();

            });

            modelBuilder.Entity<Subscription>(entity =>
            {
                entity.HasKey(s => s.Id);

                entity.Property(s => s.Name)
                    .IsRequired();

                entity.Property(s => s.Price)
                    .IsRequired();

                entity.Property(s => s.ConversionsLimit);
            });

            modelBuilder.Entity<Currency>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.Property(c => c.Name)
                    .IsRequired();

                entity.Property(c => c.Symbol)
                    .IsRequired();

                entity.Property(c => c.ConversionRate);
            });

            modelBuilder.Entity<User>()
                .HasOne(u => u.Subscription)
                .WithMany(s => s.Subscribers)
                .HasForeignKey(u => u.SubscriptionId);

            modelBuilder.Entity<Subscription>().HasData(
                new Subscription
                {
                    Id = 1,
                    Name = "Free",
                    Price = 0,
                    ConversionsLimit = 10
                },
                new Subscription
                {
                    Id = 2,
                    Name = "Trial",
                    Price = 10,
                    ConversionsLimit = 100
                },
                new Subscription
                {
                    Id = 3,
                    Name = "Pro",
                    Price = 100,
                    ConversionsLimit = null
                }
            );

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    Name = "Administrador",
                    Password = "admin",
                    SubscriptionId = 3,
                    SubscribedUntil = DateTime.UtcNow.AddMonths(12),
                    ConversionsCount = 0,
                    isDeleted = false
                }
            );

            modelBuilder.Entity<Currency>().HasData(
                new Currency
                {
                    Id = 1,
                    Name = "USD",
                    Symbol = "USD",
                    ConversionRate = 1
                },
                new Currency
                {
                    Id = 2,
                    Name = "ARS",
                    Symbol = "ARS",
                    ConversionRate = 0.001M
                },
                new Currency
                {
                    Id = 3,
                    Name = "EUR",
                    Symbol = "EUR",
                    ConversionRate = 1.09M
                }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
