using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankingSystem.DAL.Migrations.Development
{
    /// <inheritdoc />
    public partial class secondEdition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admins_Branches_BranchId",
                table: "Admins");

            migrationBuilder.DropForeignKey(
                name: "FK_Banks_Admins_GManagerId",
                table: "Banks");

            migrationBuilder.DropIndex(
                name: "IX_Managers_BranchId",
                table: "Managers");

            migrationBuilder.DropIndex(
                name: "IX_Banks_GManagerId",
                table: "Banks");

            migrationBuilder.DropIndex(
                name: "IX_Admins_BranchId",
                table: "Admins");

            migrationBuilder.DropColumn(
                name: "GManagerId",
                table: "Banks");

            migrationBuilder.RenameColumn(
                name: "BranchId",
                table: "Admins",
                newName: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_Managers_BranchId",
                table: "Managers",
                column: "BranchId",
                unique: true,
                filter: "[BranchId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Admins_BankId",
                table: "Admins",
                column: "BankId",
                unique: true,
                filter: "[BankId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Admins_Banks_BankId",
                table: "Admins",
                column: "BankId",
                principalTable: "Banks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admins_Banks_BankId",
                table: "Admins");

            migrationBuilder.DropIndex(
                name: "IX_Managers_BranchId",
                table: "Managers");

            migrationBuilder.DropIndex(
                name: "IX_Admins_BankId",
                table: "Admins");

            migrationBuilder.RenameColumn(
                name: "BankId",
                table: "Admins",
                newName: "BranchId");

            migrationBuilder.AddColumn<string>(
                name: "GManagerId",
                table: "Banks",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Managers_BranchId",
                table: "Managers",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Banks_GManagerId",
                table: "Banks",
                column: "GManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Admins_BranchId",
                table: "Admins",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Admins_Branches_BranchId",
                table: "Admins",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Banks_Admins_GManagerId",
                table: "Banks",
                column: "GManagerId",
                principalTable: "Admins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
