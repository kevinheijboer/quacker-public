using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccountService.Api.Migrations
{
    public partial class AddedAccounts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Username = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    Email = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
