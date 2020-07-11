namespace Exchangerat.Clients.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    using static Common.Constants.DataConstants;

    public class FundConfiguration : IEntityTypeConfiguration<Fund>
    {
        public void Configure(EntityTypeBuilder<Fund> builder)
        {
            builder
                .HasKey(f => f.Id);

            builder
                .Property(f => f.CardIdentityNumber)
                .HasMaxLength(CreditCardNumberLength)
                .IsFixedLength()
                .IsRequired();

            builder
                .Property(f => f.Amount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder
                .HasOne(f => f.Account)
                .WithMany(ea => ea.Funds)
                .HasForeignKey(f => f.AccountId)
                .IsRequired();

            builder
                .HasOne(f => f.Client)
                .WithMany(c => c.Funds)
                .HasForeignKey(f => f.ClientId)
                .IsRequired();

            builder
                .Property(f => f.IssuedAt)
                .IsRequired()
                .HasDefaultValueSql("getutcdate()");
        }
    }
}
