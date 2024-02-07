using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TimelineService.Api.Migrations
{
    public partial class addedMentions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Mention",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Value = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    QuackId = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mention", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mention_Quacks_QuackId",
                        column: x => x.QuackId,
                        principalTable: "Quacks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Mention_QuackId",
                table: "Mention",
                column: "QuackId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mention");
        }
    }
}
