using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankingSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class loanDocRel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FinancialDocument_CustomerId",
                table: "FinancialDocument");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialDocument_CustomerId",
                table: "FinancialDocument",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FinancialDocument_CustomerId",
                table: "FinancialDocument");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialDocument_CustomerId",
                table: "FinancialDocument",
                column: "CustomerId",
                unique: true);
        }
    }
}
