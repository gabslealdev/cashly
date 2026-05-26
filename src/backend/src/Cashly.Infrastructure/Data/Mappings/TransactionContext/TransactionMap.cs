using Cashly.Domain.CashflowContext.Entities;
using Cashly.Domain.CashflowContext.ValueObjects;
using Cashly.Domain.TransactionContext.Entity;
using Cashly.Domain.TransactionContext.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cashly.Infrastructure.Data.Mappings.TransactionContext;

public sealed class TransactionMap : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("transactions");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(x => x.Title)
            .HasConversion(title => title.Value, value => Title.Create(value))
            .HasColumnName("title")
            .HasColumnType("varchar")
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(x => x.Amount)
            .HasConversion(amount => amount.Value, value => Amount.Create(value))
            .HasColumnName("amount")
            .HasColumnType("numeric(18,2)")
            .IsRequired();
        
        builder.Property(x => x.Type)
            .HasConversion<string>()
            .HasColumnName("transaction_type")
            .HasColumnType("varchar")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Date)
            .HasColumnName("date")
            .HasColumnType("datetimeoffset")
            .IsRequired();
        
        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasColumnName("transaction_status")
            .HasColumnType("varchar")
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("datetimeoffset")
            .IsRequired();
        
        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("datetimeoffset")
            .IsRequired();

        builder.Property(x => x.CashflowId)
            .HasColumnName("cashflow_id")
            .HasColumnType("uniqueidentifier")
            .IsRequired();

        builder.HasOne<Cashflow>()
            .WithMany()
            .HasForeignKey(x => x.CashflowId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();
    }
}