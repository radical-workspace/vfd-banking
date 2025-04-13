using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankingSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class YoussefInitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Customers_CustomerId",
                table: "Cards");

            migrationBuilder.DropForeignKey(
                name: "FK_Certificates_Accounts_AccountId",
                table: "Certificates");

            migrationBuilder.DropIndex(
                name: "IX_Cards_AccountId",
                table: "Cards");

            migrationBuilder.DropIndex(
                name: "IX_Cards_CustomerId",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Certificates");

            migrationBuilder.DropColumn(
                name: "InterestRate",
                table: "Certificates");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Cards");

            migrationBuilder.AlterColumn<long>(
                name: "CertificateNumber",
                table: "Certificates",
                type: "bigint",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<int>(
                name: "AccountId",
                table: "Certificates",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "GeneralCertificateId",
                table: "Certificates",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GeneralCertificate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    InterestRate = table.Column<double>(type: "float", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralCertificate", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_GeneralCertificateId",
                table: "Certificates",
                column: "GeneralCertificateId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_AccountId",
                table: "Cards",
                column: "AccountId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Certificates_Accounts_AccountId",
                table: "Certificates",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Certificates_GeneralCertificate_GeneralCertificateId",
                table: "Certificates",
                column: "GeneralCertificateId",
                principalTable: "GeneralCertificate",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certificates_Accounts_AccountId",
                table: "Certificates");

            migrationBuilder.DropForeignKey(
                name: "FK_Certificates_GeneralCertificate_GeneralCertificateId",
                table: "Certificates");

            migrationBuilder.DropTable(
                name: "GeneralCertificate");

            migrationBuilder.DropIndex(
                name: "IX_Certificates_GeneralCertificateId",
                table: "Certificates");

            migrationBuilder.DropIndex(
                name: "IX_Cards_AccountId",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "GeneralCertificateId",
                table: "Certificates");

            migrationBuilder.AlterColumn<string>(
                name: "CertificateNumber",
                table: "Certificates",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<int>(
                name: "AccountId",
                table: "Certificates",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "Certificates",
                type: "decimal(18,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<double>(
                name: "InterestRate",
                table: "Certificates",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "Cards",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cards_AccountId",
                table: "Cards",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_CustomerId",
                table: "Cards",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_Customers_CustomerId",
                table: "Cards",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Certificates_Accounts_AccountId",
                table: "Certificates",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
