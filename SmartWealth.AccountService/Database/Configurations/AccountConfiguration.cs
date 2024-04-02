using SmartWealth.AccountService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SmartWealth.AccountService.Database.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(account => account.Id);

        builder
            .Property(account => account.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder
            .Property(account => account.AccountType)
            .IsRequired();

        builder
            .Property(transaction => transaction.UserId)
            .HasMaxLength(36)
            .IsRequired();
    }
}