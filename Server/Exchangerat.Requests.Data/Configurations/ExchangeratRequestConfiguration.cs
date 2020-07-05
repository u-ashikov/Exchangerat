namespace Exchangerat.Requests.Data.Configurations
{
    using Exchangerat.Constants;
    using Exchangerat.Requests.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ExchangeratRequestConfiguration : IEntityTypeConfiguration<ExchangeratRequest>
    {
        public void Configure(EntityTypeBuilder<ExchangeratRequest> builder)
        {
            builder
                .HasKey(er => er.Id);

            builder
                .Property(er => er.Status)
                .IsRequired();

            builder
                .Property(er => er.AccountId)
                .IsRequired(required: false);

            builder
                .HasOne(er => er.RequestType)
                .WithMany(rt => rt.Requests)
                .HasForeignKey(er => er.RequestTypeId)
                .IsRequired();

            builder
                .Property(er => er.UserId)
                .IsRequired()
                .HasMaxLength(DataConstants.UserIdMaxLength)
                .IsFixedLength();
        }
    }
}
