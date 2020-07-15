namespace Exchangerat.Requests.Data.Configurations
{
    using Exchangerat.Requests.Common.Constants;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class RequestTypeConfiguration : IEntityTypeConfiguration<RequestType>
    {
        public void Configure(EntityTypeBuilder<RequestType> builder)
        {
            builder
                .HasKey(rt => rt.Id);

            builder
                .Property(rt => rt.Type)
                .HasMaxLength(DataConstants.RequestTypeNameMaxLength)
                .IsRequired();
        }
    }
}
