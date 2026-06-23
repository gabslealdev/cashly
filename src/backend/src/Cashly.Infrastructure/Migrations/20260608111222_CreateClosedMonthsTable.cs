using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cashly.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateClosedMonthsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "closed_months",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    cashflow_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    period = table.Column<int>(type: "int", nullable: false),
                    balance = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    status = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    closed_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_closed_months", x => x.id);
                    table.ForeignKey(
                        name: "FK_closed_months_cashflows_cashflow_id",
                        column: x => x.cashflow_id,
                        principalTable: "cashflows",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_closed_months_cashflow_id_period",
                table: "closed_months",
                columns: new[] { "cashflow_id", "period" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "closed_months");
        }
    }
}
