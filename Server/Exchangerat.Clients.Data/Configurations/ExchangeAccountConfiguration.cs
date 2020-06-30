namespace Exchangerat.Clients.Data.Configurations
{
    using Common.Constants;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class ExchangeAccountConfiguration : IEntityTypeConfiguration<ExchangeAccount>
    {
        public void Configure(EntityTypeBuilder<ExchangeAccount> builder)
        {
            builder
                .HasKey(ea => ea.Id);

            builder
                .Property(ea => ea.IdentityNumber)
                .IsRequired()
                .HasMaxLength(DataConstants.ExchangeAccountIdentifierMaxLength)
                .IsFixedLength();

            builder
                .HasIndex(ea => ea.IdentityNumber)
                .IsUnique();

            builder
                .Property(ea => ea.IsActive)
                .HasDefaultValue(true)
                .IsRequired();

            builder
                .HasOne(ea => ea.Owner)
                .WithMany(o => o.ExchangeAccounts)
                .HasForeignKey(ea => ea.OwnerId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Property(ea => ea.Balance)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder
                .HasOne(ea => ea.Type)
                .WithMany(t => t.ExchangeAccounts)
                .HasForeignKey(ea => ea.TypeId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
