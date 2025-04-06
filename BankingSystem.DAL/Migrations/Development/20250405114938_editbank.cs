using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankingSystem.DAL.Migrations.Development
{
    /// <inheritdoc />
    public partial class editbank : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Banks_Managers_ManagerId",
                table: "Banks");

            migrationBuilder.RenameColumn(
                name: "ManagerId",
                table: "Banks",
                newName: "GManagerId");

            migrationBuilder.RenameIndex(
                name: "IX_Banks_ManagerId",
                table: "Banks",
                newName: "IX_Banks_GManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Banks_Admins_GManagerId",
                table: "Banks",
                column: "GManagerId",
                principalTable: "Admins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Banks_Admins_GManagerId",
                table: "Banks");

            migrationBuilder.RenameColumn(
                name: "GManagerId",
                table: "Banks",
                newName: "ManagerId");

            migrationBuilder.RenameIndex(
                name: "IX_Banks_GManagerId",
                table: "Banks",
                newName: "IX_Banks_ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Banks_Managers_ManagerId",
                table: "Banks",
                column: "ManagerId",
                principalTable: "Managers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
