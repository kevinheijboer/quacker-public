using Microsoft.EntityFrameworkCore.Migrations;

namespace AccountService.Api.Migrations
{
    public partial class addeduserattributes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Accounts",
                type: "varchar(50) CHARACTER SET utf8mb4",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "Accounts",
                type: "varchar(160) CHARACTER SET utf8mb4",
                maxLength: 160,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Accounts",
                type: "varchar(30) CHARACTER SET utf8mb4",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "Accounts",
                type: "varchar(100) CHARACTER SET utf8mb4",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bio",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "Accounts");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Accounts",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50) CHARACTER SET utf8mb4",
                oldMaxLength: 50,
                oldNullable: true);
        }
    }
}
