using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartWealth.TransactionService.Models;

namespace SmartWealth.TransactionService.Database.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(transaction => transaction.Id);

        builder
            .Property(transaction => transaction.CategoryId)
            .IsRequired();

        builder
            .Property(transaction => transaction.Note)
            .HasMaxLength(100)
            .IsRequired();

        builder
            .Property(transaction => transaction.AccountId)
            .IsRequired();

        builder
            .Property(transaction => transaction.Amount)
            .IsRequired();

        builder
            .Property(transaction => transaction.CreatedAt)
            .IsRequired();
    }
}