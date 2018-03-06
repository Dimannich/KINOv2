using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace KINOv2.Data.Migrations
{
    public partial class _020318 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Films",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GlobalRating",
                table: "Films",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LocalRating",
                table: "Films",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrailerLink",
                table: "Films",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfileImage",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SurName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    LINK = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ApplicationUserId = table.Column<string>(nullable: true),
                    BaseCommentLINK = table.Column<int>(nullable: true),
                    FilmLINK = table.Column<int>(nullable: true),
                    Text = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.LINK);
                    table.ForeignKey(
                        name: "FK_Comments_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_Comments_BaseCommentLINK",
                        column: x => x.BaseCommentLINK,
                        principalTable: "Comments",
                        principalColumn: "LINK",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_Films_FilmLINK",
                        column: x => x.FilmLINK,
                        principalTable: "Films",
                        principalColumn: "LINK",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    LINK = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ApplicationUserId = table.Column<string>(nullable: true),
                    CommentLINK = table.Column<int>(nullable: true),
                    FilmLINK = table.Column<int>(nullable: true),
                    Value = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => x.LINK);
                    table.ForeignKey(
                        name: "FK_Ratings_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ratings_Comments_CommentLINK",
                        column: x => x.CommentLINK,
                        principalTable: "Comments",
                        principalColumn: "LINK",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ratings_Films_FilmLINK",
                        column: x => x.FilmLINK,
                        principalTable: "Films",
                        principalColumn: "LINK",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Films_ApplicationUserId",
                table: "Films",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ApplicationUserId",
                table: "Comments",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_BaseCommentLINK",
                table: "Comments",
                column: "BaseCommentLINK");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_FilmLINK",
                table: "Comments",
                column: "FilmLINK");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_ApplicationUserId",
                table: "Ratings",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_CommentLINK",
                table: "Ratings",
                column: "CommentLINK");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_FilmLINK",
                table: "Ratings",
                column: "FilmLINK");

            migrationBuilder.AddForeignKey(
                name: "FK_Films_AspNetUsers_ApplicationUserId",
                table: "Films",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Films_AspNetUsers_ApplicationUserId",
                table: "Films");

            migrationBuilder.DropTable(
                name: "Ratings");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Films_ApplicationUserId",
                table: "Films");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Films");

            migrationBuilder.DropColumn(
                name: "GlobalRating",
                table: "Films");

            migrationBuilder.DropColumn(
                name: "LocalRating",
                table: "Films");

            migrationBuilder.DropColumn(
                name: "TrailerLink",
                table: "Films");

            migrationBuilder.DropColumn(
                name: "Age",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "City",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProfileImage",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SurName",
                table: "AspNetUsers");
        }
    }
}
