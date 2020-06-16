namespace Exchangerat.Data.Configurations
{
    using Common.Constants;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder
                .HasKey(t => t.Id);

            builder
                .HasOne(t => t.SenderAccount)
                .WithMany(sa => sa.SentTransactions)
                .HasForeignKey(t => t.SenderAccountId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(t => t.ReceiverAccount)
                .WithMany(ra => ra.ReceivedTransactions)
                .HasForeignKey(t => t.ReceiverAccountId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Property(t => t.Amount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder
                .Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(DataConstants.TransactionDescriptionMaxLength);
        }
    }
}
