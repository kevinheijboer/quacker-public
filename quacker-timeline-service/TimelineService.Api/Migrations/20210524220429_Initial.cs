using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TimelineService.Api.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Quacks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Message = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Username = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quacks", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Quacks");
        }
    }
}
