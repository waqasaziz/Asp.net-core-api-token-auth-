using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Entities
{
    using Helpers;
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(x => x.Id);

            builder
               .Property(x => x.CardNumber)
               .IsRequired()
               .HasMaxLength(ModelConstants.Card.NumberMaxLength);

            builder
                .Property(x => x.NameOnCard)
                .IsRequired()
                .HasMaxLength(ModelConstants.Card.NameMaxLength);

            builder.Property(x => x.ExpiryDate)
                .IsRequired()
                .HasMaxLength(ModelConstants.Card.ExpiryDateMaxLength);

            builder.Property(x => x.Amount)
                .IsRequired();

            builder.HasOne(x => x.Merchant)
                .WithMany(x => x.Payments);
        }
    }
}
