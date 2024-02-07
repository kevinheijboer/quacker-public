using Microsoft.EntityFrameworkCore.Migrations;

namespace TimelineService.Api.Migrations
{
    public partial class RemovedUnrelevantUserData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bio",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "Users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "Users",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Users",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Users",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "Users",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
