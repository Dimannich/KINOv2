using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace KINOv2.Data.Migrations
{
    public partial class _050318 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Comments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Comments");
        }
    }
}
