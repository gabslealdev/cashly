using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cashly.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTransactionCashflowDateIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_transactions_cashflow_id",
                table: "transactions");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_cashflow_id_date",
                table: "transactions",
                columns: new[] { "cashflow_id", "date" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_transactions_cashflow_id_date",
                table: "transactions");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_cashflow_id",
                table: "transactions",
                column: "cashflow_id");
        }
    }
}
