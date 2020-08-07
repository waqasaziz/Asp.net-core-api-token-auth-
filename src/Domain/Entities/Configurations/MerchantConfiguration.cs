using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Entities
{
    using Helpers;
    public class MerchantConfiguration : IEntityTypeConfiguration<Merchant>
    {
        public void Configure(EntityTypeBuilder<Merchant> builder)
        {
            builder.HasKey(x => x.Id);

            builder
               .Property(x => x.Username)
               .IsRequired()
               .HasMaxLength(ModelConstants.Merchant.NameMaxLength);

            builder
                .Property(x => x.Username)
                .IsRequired()
                .HasMaxLength(ModelConstants.Merchant.NameMaxLength);

            builder.Property(x => x.Username).IsRequired();
            builder.HasIndex(x => x.Username).IsUnique();

            builder.Property(x => x.PasswordHash)
                .IsRequired()
                .HasMaxLength(ModelConstants.Merchant.PasswordMaxLength);

            builder.Property(x => x.PasswordSalt)
              .IsRequired()
              .HasMaxLength(ModelConstants.Merchant.SaltMaxLength);

            builder.OwnsMany(x => x.RefreshTokens, o =>
            {
                o.Property(p => p.Token).IsRequired();
                o.Property(p => p.CreatedOn).IsRequired();
                o.Property(p => p.CreatedByIP).IsRequired();
            });
        }
    }
}
