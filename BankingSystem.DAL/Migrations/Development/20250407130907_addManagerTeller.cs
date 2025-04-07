using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankingSystem.DAL.Migrations.Development
{
    /// <inheritdoc />
    public partial class addManagerTeller : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ManagerId",
                table: "Tellers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tellers_ManagerId",
                table: "Tellers",
                column: "ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tellers_Managers_ManagerId",
                table: "Tellers",
                column: "ManagerId",
                principalTable: "Managers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tellers_Managers_ManagerId",
                table: "Tellers");

            migrationBuilder.DropIndex(
                name: "IX_Tellers_ManagerId",
                table: "Tellers");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                table: "Tellers");
        }
    }
}
