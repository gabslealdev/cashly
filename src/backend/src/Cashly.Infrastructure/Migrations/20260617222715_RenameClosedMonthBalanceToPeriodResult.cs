using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cashly.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameClosedMonthBalanceToPeriodResult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "balance",
                table: "closed_months",
                newName: "period_result");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "period_result",
                table: "closed_months",
                newName: "balance");
        }
    }
}
