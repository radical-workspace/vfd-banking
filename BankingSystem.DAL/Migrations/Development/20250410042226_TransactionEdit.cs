using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankingSystem.DAL.Migrations.Development
{
    /// <inheritdoc />
    public partial class TransactionEdit : Migration
    {
        /// <inheritdoc />
protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerID",
                table: "Transactions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CustomerID",
                table: "Transactions",
                column: "CustomerID");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Customers_CustomerID",
                table: "Transactions",
                column: "CustomerID",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Customers_CustomerID",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_CustomerID",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "CustomerID",
                table: "Transactions");

            migrationBuilder.AddColumn<string>(
                name: "MyCustomerId",
                table: "Transactions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_MyCustomerId",
                table: "Transactions",
                column: "MyCustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Customers_MyCustomerId",
                table: "Transactions",
                column: "MyCustomerId",
                principalTable: "Customers",
                principalColumn: "Id");
        }
    }
}
