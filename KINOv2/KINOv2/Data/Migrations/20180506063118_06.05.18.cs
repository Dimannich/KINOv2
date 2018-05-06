using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace KINOv2.Data.Migrations
{
    public partial class _060518 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Films_FilmLINK",
                table: "Comments");

            migrationBuilder.AlterColumn<int>(
                name: "FilmLINK",
                table: "Comments",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "SelectedFilmsVisible",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "PersonalInfoVisible",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Films_FilmLINK",
                table: "Comments",
                column: "FilmLINK",
                principalTable: "Films",
                principalColumn: "LINK",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Films_FilmLINK",
                table: "Comments");

            migrationBuilder.AlterColumn<int>(
                name: "FilmLINK",
                table: "Comments",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<bool>(
                name: "SelectedFilmsVisible",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<bool>(
                name: "PersonalInfoVisible",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Films_FilmLINK",
                table: "Comments",
                column: "FilmLINK",
                principalTable: "Films",
                principalColumn: "LINK",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
