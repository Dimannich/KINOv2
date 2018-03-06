using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace KINOv2.Data.Migrations
{
    public partial class _040318 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Films",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "About",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PersonalInfoVisible",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SelectedFilmsVisible",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Films");

            migrationBuilder.DropColumn(
                name: "About",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PersonalInfoVisible",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SelectedFilmsVisible",
                table: "AspNetUsers");
        }
    }
}
