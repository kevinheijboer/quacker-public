using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QuackService.Api.Migrations
{
    public partial class AddedQuacks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Quacks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Message = table.Column<string>(type: "varchar(140) CHARACTER SET utf8mb4", maxLength: 140, nullable: true)
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
