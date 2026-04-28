using Cashly.Domain.CashflowContext.Entities;
using Cashly.Domain.CashflowContext.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cashly.Infrastructure.Data.Mappings.CashflowContext;

public sealed class CashflowMap : IEntityTypeConfiguration<Cashflow>
{
    public void Configure(EntityTypeBuilder<Cashflow> builder)
    {
        builder.ToTable("cashflows");
        
        builder.HasKey(c => c.Id)
            .HasName("PK_cashflows");
        
        builder.Property(c => c.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Title)
            .HasConversion(title => title.Value, value => Title.Create(value))
            .HasColumnName("title")
            .HasColumnType("varchar")
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(c => c.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("datetimeoffset")
            .IsRequired();

        builder.Property(c => c.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("datetimeoffset");
        
        builder.HasMany(c => c.CashflowMembers)
            .WithOne()
            .HasForeignKey(cm => cm.CashflowId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Navigation(c => c.CashflowMembers)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
        
    }
}