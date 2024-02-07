using Microsoft.EntityFrameworkCore.Migrations;

namespace AccountService.Api.Migrations
{
    public partial class addedNameAttribute : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Accounts",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50) CHARACTER SET utf8mb4",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Accounts",
                type: "varchar(50) CHARACTER SET utf8mb4",
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Accounts");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Accounts",
                type: "varchar(50) CHARACTER SET utf8mb4",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);
        }
    }
}
