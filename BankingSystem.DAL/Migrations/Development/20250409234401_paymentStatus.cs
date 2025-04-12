using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankingSystem.DAL.Migrations.Development
{
    /// <inheritdoc />
    public partial class paymentStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Payment",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Payment");
        }
    }
}
