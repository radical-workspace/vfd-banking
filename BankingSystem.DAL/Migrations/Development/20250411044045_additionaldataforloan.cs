using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankingSystem.DAL.Migrations.Development
{
    /// <inheritdoc />
    public partial class additionaldataforloan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Accounts_AccountId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Customers_CustomerId",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Transactions",
                newName: "MyCustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_CustomerId",
                table: "Transactions",
                newName: "IX_Transactions_MyCustomerId");

            migrationBuilder.AlterColumn<int>(
                name: "AccountId",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AccountDistenationNumber",
                table: "Transactions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "Asset",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EstimatedValue = table.Column<decimal>(type: "decimal(18,4)", nullable: false, defaultValue: 0m),
                    MyCustomerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asset", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Asset_Customers_MyCustomerId",
                        column: x => x.MyCustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "IncomeSource",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MonthlyAmount = table.Column<decimal>(type: "decimal(18,4)", nullable: false, defaultValue: 0m),
                    MyCustomerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomeSource", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncomeSource_Customers_MyCustomerId",
                        column: x => x.MyCustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Asset_MyCustomerId",
                table: "Asset",
                column: "MyCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeSource_MyCustomerId",
                table: "IncomeSource",
                column: "MyCustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Accounts_AccountId",
                table: "Transactions",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Customers_MyCustomerId",
                table: "Transactions",
                column: "MyCustomerId",
                principalTable: "Customers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Accounts_AccountId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Customers_MyCustomerId",
                table: "Transactions");

            migrationBuilder.DropTable(
                name: "Asset");

            migrationBuilder.DropTable(
                name: "IncomeSource");

            migrationBuilder.DropColumn(
                name: "AccountDistenationNumber",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "MyCustomerId",
                table: "Transactions",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_MyCustomerId",
                table: "Transactions",
                newName: "IX_Transactions_CustomerId");

            migrationBuilder.AlterColumn<int>(
                name: "AccountId",
                table: "Transactions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Accounts_AccountId",
                table: "Transactions",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Customers_CustomerId",
                table: "Transactions",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id");
        }
    }
}
