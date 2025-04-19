using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankingSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ccc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LoanId",
                table: "FinancialDocument",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FinancialDocument_LoanId",
                table: "FinancialDocument",
                column: "LoanId");

            migrationBuilder.AddForeignKey(
                name: "FK_FinancialDocument_Loans_LoanId",
                table: "FinancialDocument",
                column: "LoanId",
                principalTable: "Loans",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinancialDocument_Loans_LoanId",
                table: "FinancialDocument");

            migrationBuilder.DropIndex(
                name: "IX_FinancialDocument_LoanId",
                table: "FinancialDocument");

            migrationBuilder.DropColumn(
                name: "LoanId",
                table: "FinancialDocument");
        }
    }
}
