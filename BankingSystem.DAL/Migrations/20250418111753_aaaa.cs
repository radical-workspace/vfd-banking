using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankingSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class aaaa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinancialDocument_Customer",
                table: "FinancialDocument");

            migrationBuilder.DropForeignKey(
                name: "FK_FinancialDocument_Customers_CustomerId1",
                table: "FinancialDocument");

            migrationBuilder.DropIndex(
                name: "IX_FinancialDocument_CustomerId1",
                table: "FinancialDocument");

            migrationBuilder.DropColumn(
                name: "CustomerId1",
                table: "FinancialDocument");

            migrationBuilder.AddForeignKey(
                name: "FK_FinancialDocument_Customers_CustomerId",
                table: "FinancialDocument",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinancialDocument_Customers_CustomerId",
                table: "FinancialDocument");

            migrationBuilder.AddColumn<string>(
                name: "CustomerId1",
                table: "FinancialDocument",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FinancialDocument_CustomerId1",
                table: "FinancialDocument",
                column: "CustomerId1",
                unique: true,
                filter: "[CustomerId1] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_FinancialDocument_Customer",
                table: "FinancialDocument",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FinancialDocument_Customers_CustomerId1",
                table: "FinancialDocument",
                column: "CustomerId1",
                principalTable: "Customers",
                principalColumn: "Id");
        }
    }
}
