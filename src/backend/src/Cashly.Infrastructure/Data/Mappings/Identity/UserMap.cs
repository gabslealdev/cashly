using Cashly.Domain.IdentityContext.Entities;
using Cashly.Domain.IdentityContext.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cashly.Infrastructure.Data.Mappings.Identity
{
    public sealed class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");

            builder.HasKey(x => x.Id)
                .HasName("PK_users");

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedNever();

            builder.OwnsOne(x => x.Name, name => 
            {
                name.Property(n => n.FirstName)
                .HasColumnName("first_name")
                .HasMaxLength(80)
                .IsRequired();

                name.Property(n => n.LastName)
                .HasColumnName("last_name")
                .HasMaxLength(80)
                .IsRequired();
            });

            builder.Property(n => n.Email)
                .HasConversion(email => email.Value, value => Email.Create(value))
                .HasColumnName("email")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(x => x.PasswordHash)
                .HasConversion(passwordHash => passwordHash.Value, value => PasswordHash.Create(value))
                .HasColumnName("password_hash")
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("datetimeoffset")
                .IsRequired();

            builder.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at")
                .HasColumnType("datetimeoffset");

            builder.HasIndex(x => x.Email)
                .IsUnique();
        }
    }
}
