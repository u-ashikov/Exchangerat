namespace Exchangerat.Data.Configurations
{
    using Common.Constants;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class ExchangeAccountTypeConfiguration : IEntityTypeConfiguration<ExchangeAccountType>
    {
        public void Configure(EntityTypeBuilder<ExchangeAccountType> builder)
        {
            builder
                .HasKey(t => t.Id);

            builder
                .Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(DataConstants.ExchangeAccountTypeNameMaxLength);

            builder
                .Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(DataConstants.ExchangeAccountTypeDescriptionMaxLength);
        }
    }
}
