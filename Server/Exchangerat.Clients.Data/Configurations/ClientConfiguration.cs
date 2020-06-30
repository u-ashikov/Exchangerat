namespace Exchangerat.Clients.Data.Configurations
{
    using Common.Constants;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder
                .HasKey(c => c.Id);

            builder
                .Property(c => c.FirstName)
                .HasMaxLength(DataConstants.ClientFirstNameMaxLength)
                .IsRequired();

            builder
                .Property(c => c.LastName)
                .HasMaxLength(DataConstants.ClientLastNameMaxLength)
                .IsRequired();

            builder
                .Property(c => c.Address)
                .IsRequired()
                .HasMaxLength(DataConstants.ClientAddressMaxLength);

            builder
                .Property(c => c.UserId)
                .IsRequired()
                .HasMaxLength(DataConstants.ClientUserIdMaxLength)
                .IsFixedLength();
        }
    }
}
