namespace Exchangerat.Data.Configurations
{
    using Common.Constants;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .HasKey(u => u.Id);

            builder
                .Property(u => u.Address)
                .IsRequired()
                .HasMaxLength(DataConstants.UserAddressMaxLength);
        }
    }
}
