using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VerticalSliceMinimalApi.Domain.Account.ValueObjects;
using VerticalSliceMinimalApi.Entities.Account;

namespace VerticalSliceMinimalApi.Persistence.EntityFramework.Configurations;

internal class AccountEntityTypeConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(c => c.Id);
        //builder.Property(c => c.Id).HasConversion<Guid>();

        builder.Property(p => p.Id).HasConversion(
            id => id.Value,
            value => AccountId.Create(value));

        builder.HasIndex(c => c.Email)
        .IsUnique();

        builder.Property(p => p.Email).HasConversion(
            email => email.Value,
            value => Email.Create(value));

        builder.Property(p => p.UserName).HasConversion(
            userName => userName.Value,
            value => UserName.Create(value));

        builder.Property(p => p.Mobile).HasConversion(
            mobile => mobile.Value,
            value => Mobile.Create(value));

        builder.Property(c => c.AccountType).HasConversion<int>();

        builder.Property(p => p.CityId).HasConversion(
            cityId => cityId.Value,
            value => new CityId(value));
    }
}
