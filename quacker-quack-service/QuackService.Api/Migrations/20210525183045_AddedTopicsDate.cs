using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QuackService.Api.Migrations
{
    public partial class AddedTopicsDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Topic_Quacks_QuackId",
                table: "Topic");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Topic",
                table: "Topic");

            migrationBuilder.RenameTable(
                name: "Topic",
                newName: "Topics");

            migrationBuilder.RenameIndex(
                name: "IX_Topic_QuackId",
                table: "Topics",
                newName: "IX_Topics_QuackId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Mention",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Topics",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Topics",
                table: "Topics",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Topics_Quacks_QuackId",
                table: "Topics",
                column: "QuackId",
                principalTable: "Quacks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Topics_Quacks_QuackId",
                table: "Topics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Topics",
                table: "Topics");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Mention");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Topics");

            migrationBuilder.RenameTable(
                name: "Topics",
                newName: "Topic");

            migrationBuilder.RenameIndex(
                name: "IX_Topics_QuackId",
                table: "Topic",
                newName: "IX_Topic_QuackId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Topic",
                table: "Topic",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Topic_Quacks_QuackId",
                table: "Topic",
                column: "QuackId",
                principalTable: "Quacks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
