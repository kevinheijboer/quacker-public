using Microsoft.EntityFrameworkCore.Migrations;

namespace TimelineService.Api.Migrations
{
    public partial class addedMentions2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mention_Quacks_QuackId",
                table: "Mention");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Mention",
                table: "Mention");

            migrationBuilder.RenameTable(
                name: "Mention",
                newName: "Mentions");

            migrationBuilder.RenameIndex(
                name: "IX_Mention_QuackId",
                table: "Mentions",
                newName: "IX_Mentions_QuackId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Mentions",
                table: "Mentions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Mentions_Quacks_QuackId",
                table: "Mentions",
                column: "QuackId",
                principalTable: "Quacks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mentions_Quacks_QuackId",
                table: "Mentions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Mentions",
                table: "Mentions");

            migrationBuilder.RenameTable(
                name: "Mentions",
                newName: "Mention");

            migrationBuilder.RenameIndex(
                name: "IX_Mentions_QuackId",
                table: "Mention",
                newName: "IX_Mention_QuackId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Mention",
                table: "Mention",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Mention_Quacks_QuackId",
                table: "Mention",
                column: "QuackId",
                principalTable: "Quacks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
