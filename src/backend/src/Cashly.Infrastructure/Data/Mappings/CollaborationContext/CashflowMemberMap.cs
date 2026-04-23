using Cashly.Domain.CollaborationContext.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cashly.Infrastructure.Data.Mappings.CollaborationContext;

public class CashflowMemberMap : IEntityTypeConfiguration<CashflowMember>
{
    public void Configure(EntityTypeBuilder<CashflowMember> builder)
    {
        builder.ToTable("cashflow_member");
        
        builder.HasKey(cm => cm.Id)
            .HasName("PK_cashflow_member");

        builder.Property(cm => cm.Id)
            .ValueGeneratedNever();

        builder.Property(cm => cm.CashflowId)
            .HasColumnName("cashflow_id")
            .IsRequired();
        
        builder.Property(cm => cm.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(cm => cm.Role)
            .HasColumnName("role")
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();
        
        builder.Property(cm => cm.JoinedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.HasIndex(cm => new { cm.CashflowId, cm.UserId })
            .IsUnique();

        builder.HasIndex(cm => new { cm.CashflowId, cm.Role });
        
    }
}