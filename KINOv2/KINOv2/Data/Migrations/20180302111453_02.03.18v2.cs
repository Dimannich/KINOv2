using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace KINOv2.Data.Migrations
{
    public partial class _020318v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Films_AspNetUsers_ApplicationUserId",
                table: "Films");

            migrationBuilder.DropIndex(
                name: "IX_Films_ApplicationUserId",
                table: "Films");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Films");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Films",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Films_ApplicationUserId",
                table: "Films",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Films_AspNetUsers_ApplicationUserId",
                table: "Films",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
