using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TimelineService.Api.Migrations
{
    public partial class addedMentionsForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mentions_Quacks_QuackId",
                table: "Mentions");

            migrationBuilder.AlterColumn<Guid>(
                name: "QuackId",
                table: "Mentions",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Mentions_Quacks_QuackId",
                table: "Mentions",
                column: "QuackId",
                principalTable: "Quacks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mentions_Quacks_QuackId",
                table: "Mentions");

            migrationBuilder.AlterColumn<Guid>(
                name: "QuackId",
                table: "Mentions",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)");

            migrationBuilder.AddForeignKey(
                name: "FK_Mentions_Quacks_QuackId",
                table: "Mentions",
                column: "QuackId",
                principalTable: "Quacks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
