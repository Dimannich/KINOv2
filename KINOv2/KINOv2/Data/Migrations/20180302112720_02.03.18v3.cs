using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace KINOv2.Data.Migrations
{
    public partial class _020318v3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FilmUser",
                columns: table => new
                {
                    ApplicationUserId = table.Column<string>(nullable: false),
                    FilmLINK = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilmUser", x => new { x.ApplicationUserId, x.FilmLINK });
                    table.ForeignKey(
                        name: "FK_FilmUser_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FilmUser_Films_FilmLINK",
                        column: x => x.FilmLINK,
                        principalTable: "Films",
                        principalColumn: "LINK",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FilmUser_FilmLINK",
                table: "FilmUser",
                column: "FilmLINK");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FilmUser");
        }
    }
}
