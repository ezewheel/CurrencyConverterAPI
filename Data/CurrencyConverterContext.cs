using Common.Enums;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class CurrencyConverterContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Conversion> Conversions { get; set; }

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

                entity.Property(c => c.ConversionRate)
                    .IsRequired();

                entity.Property(c => c.CountryCode);

                entity.Property(c => c.IsDeleted)
                    .IsRequired();
            });

            modelBuilder.Entity<Conversion>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.Property(c => c.Amount)
                    .IsRequired();

                entity.Property(c => c.Result)
                    .IsRequired();

                entity.Property(c => c.Date)
                    .IsRequired();
            });

            modelBuilder.Entity<User>()
                .HasOne(u => u.Subscription)
                .WithMany()
                .HasForeignKey(u => u.SubscriptionId);

            modelBuilder.Entity<Conversion>()
                .HasOne(c => c.FromCurrency)
                .WithMany()
                .HasForeignKey(c => c.FromCurrencyId);

            modelBuilder.Entity<Conversion>()
                .HasOne(c => c.ToCurrency)
                .WithMany()
                .HasForeignKey(c => c.ToCurrencyId);

            modelBuilder.Entity<Conversion>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<Conversion>()
                .HasIndex(c => c.UserId)
                .HasDatabaseName("IX_Conversions_UserId");

            modelBuilder.Entity<Subscription>().HasData(
                new Subscription
                {
                    Id = SubscriptionTypeEnum.Free,
                    Name = "Free",
                    Price = 0,
                    ConversionsLimit = 10
                },
                new Subscription
                {
                    Id = SubscriptionTypeEnum.Trial,
                    Name = "Trial",
                    Price = 10,
                    ConversionsLimit = 100
                },
                new Subscription
                {
                    Id = SubscriptionTypeEnum.Pro,
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
                    SubscriptionId = SubscriptionTypeEnum.Pro,
                    SubscribedUntil = DateTime.UtcNow.AddMonths(12),
                    ConversionsCount = 0,
                    isDeleted = false
                }
            );

            modelBuilder.Entity<Currency>().HasData(
                new Currency
                {
                    Id = 1,
                    Name = "Dólar estadounidense",
                    Symbol = "USD",
                    ConversionRate = 1,
                    CountryCode = "US",
                    IsDeleted = false
                },
                new Currency
                {
                    Id = 2,
                    Name = "Peso argentino",
                    Symbol = "ARS",
                    ConversionRate = 0.001M,
                    CountryCode = "AR",
                    IsDeleted = false

                },
                new Currency
                {
                    Id = 3,
                    Name = "Euro",
                    Symbol = "EUR",
                    ConversionRate = 1.09M,
                    CountryCode = "EU",
                    IsDeleted = false
                }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
