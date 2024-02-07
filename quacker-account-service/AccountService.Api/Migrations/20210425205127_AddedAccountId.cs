using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AccountService.Api.Migrations
{
    public partial class AddedAccountId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Accounts",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Accounts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts",
                column: "UserId");
        }
    }
}
