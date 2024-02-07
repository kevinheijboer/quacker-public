using Microsoft.EntityFrameworkCore.Migrations;

namespace QuackService.Api.Migrations
{
    public partial class addedusername : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Quacks",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "Quacks");
        }
    }
}
