using Cashly.Domain.CashflowContext.Entities;
using Cashly.Domain.CashflowContext.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cashly.Infrastructure.Data.Mappings.CashflowContext;

public sealed class ClosedMonthMap : IEntityTypeConfiguration<ClosedMonth>
{
    public void Configure(EntityTypeBuilder<ClosedMonth> builder)
    {
        builder.ToTable("closed_months");

        builder.HasKey(closedMonth => closedMonth.Id)
            .HasName("PK_closed_months");

        builder.Property(closedMonth => closedMonth.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(closedMonth => closedMonth.CashflowId)
            .HasColumnName("cashflow_id")
            .HasColumnType("uniqueidentifier")
            .IsRequired();

        builder.Property(closedMonth => closedMonth.Period)
            .HasConversion(
                period => period.Year * 100 + period.Month,
                value => Period.Create(value / 100, value % 100))
            .HasColumnName("period")
            .HasColumnType("int")
            .IsRequired();

        builder.Property(closedMonth => closedMonth.Balance)
            .HasConversion(
                money => money.Value,
                value => Money.Create(value))
            .HasColumnName("balance")
            .HasColumnType("numeric(18,2)")
            .IsRequired();

        builder.Property(closedMonth => closedMonth.Status)
            .HasConversion<string>()
            .HasColumnName("status")
            .HasColumnType("varchar")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(closedMonth => closedMonth.ClosedAt)
            .HasColumnName("closed_at")
            .HasColumnType("datetimeoffset")
            .IsRequired();

        builder.HasIndex(closedMonth => new { closedMonth.CashflowId, closedMonth.Period })
            .IsUnique();
    }
}
