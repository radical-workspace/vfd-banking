using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankingSystem.DAL.Migrations.Development
{
    /// <inheritdoc />
    public partial class addingAccountrelationToSupportTicket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Asset_Customers_MyCustomerId",
                table: "Asset");

            migrationBuilder.DropForeignKey(
                name: "FK_IncomeSource_Customers_MyCustomerId",
                table: "IncomeSource");

            migrationBuilder.DropIndex(
                name: "IX_IncomeSource_MyCustomerId",
                table: "IncomeSource");

            migrationBuilder.DropIndex(
                name: "IX_Asset_MyCustomerId",
                table: "Asset");

            migrationBuilder.DropColumn(
                name: "MyCustomerId",
                table: "IncomeSource");

            migrationBuilder.DropColumn(
                name: "MyCustomerId",
                table: "Asset");

            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "SupportTickets",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FinancialDocument",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FileData = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialDocument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinancialDocument_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SupportTickets_AccountId",
                table: "SupportTickets",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialDocument_CustomerId",
                table: "FinancialDocument",
                column: "CustomerId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SupportTickets_Accounts_AccountId",
                table: "SupportTickets",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupportTickets_Accounts_AccountId",
                table: "SupportTickets");

            migrationBuilder.DropTable(
                name: "FinancialDocument");

            migrationBuilder.DropIndex(
                name: "IX_SupportTickets_AccountId",
                table: "SupportTickets");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "SupportTickets");

            migrationBuilder.AddColumn<string>(
                name: "MyCustomerId",
                table: "IncomeSource",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MyCustomerId",
                table: "Asset",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IncomeSource_MyCustomerId",
                table: "IncomeSource",
                column: "MyCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Asset_MyCustomerId",
                table: "Asset",
                column: "MyCustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Asset_Customers_MyCustomerId",
                table: "Asset",
                column: "MyCustomerId",
                principalTable: "Customers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_IncomeSource_Customers_MyCustomerId",
                table: "IncomeSource",
                column: "MyCustomerId",
                principalTable: "Customers",
                principalColumn: "Id");
        }
    }
}
