using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace KINOv2.Data.Migrations
{
    public partial class _240218 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserRoles_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles");

            migrationBuilder.CreateTable(
                name: "AgeLimits",
                columns: table => new
                {
                    LINK = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amout = table.Column<int>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgeLimits", x => x.LINK);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    LINK = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.LINK);
                });

            migrationBuilder.CreateTable(
                name: "Directors",
                columns: table => new
                {
                    LINK = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Surname = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Directors", x => x.LINK);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    LINK = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.LINK);
                });

            migrationBuilder.CreateTable(
                name: "Halls",
                columns: table => new
                {
                    LINK = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Image = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    SeatsNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Halls", x => x.LINK);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    LINK = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ApplicationUserId = table.Column<string>(nullable: true),
                    Cost = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: true),
                    ValidationKey = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.LINK);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Films",
                columns: table => new
                {
                    LINK = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AgeLimitLINK = table.Column<int>(nullable: true),
                    Archived = table.Column<bool>(nullable: true),
                    CountryLINK = table.Column<int>(nullable: true),
                    DirectorLINK = table.Column<int>(nullable: true),
                    Duration = table.Column<string>(nullable: false),
                    GenreLINK = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Poster = table.Column<string>(nullable: false),
                    ReleaseYear = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Films", x => x.LINK);
                    table.ForeignKey(
                        name: "FK_Films_AgeLimits_AgeLimitLINK",
                        column: x => x.AgeLimitLINK,
                        principalTable: "AgeLimits",
                        principalColumn: "LINK",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Films_Countries_CountryLINK",
                        column: x => x.CountryLINK,
                        principalTable: "Countries",
                        principalColumn: "LINK",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Films_Directors_DirectorLINK",
                        column: x => x.DirectorLINK,
                        principalTable: "Directors",
                        principalColumn: "LINK",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Films_Genres_GenreLINK",
                        column: x => x.GenreLINK,
                        principalTable: "Genres",
                        principalColumn: "LINK",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    LINK = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Archived = table.Column<bool>(nullable: true),
                    Cost = table.Column<int>(nullable: false),
                    FilmLINK = table.Column<int>(nullable: false),
                    HallLINK = table.Column<int>(nullable: false),
                    SessionTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.LINK);
                    table.ForeignKey(
                        name: "FK_Sessions_Films_FilmLINK",
                        column: x => x.FilmLINK,
                        principalTable: "Films",
                        principalColumn: "LINK",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sessions_Halls_HallLINK",
                        column: x => x.HallLINK,
                        principalTable: "Halls",
                        principalColumn: "LINK",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Seats",
                columns: table => new
                {
                    LINK = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IsBooked = table.Column<bool>(nullable: false),
                    Number = table.Column<int>(nullable: false),
                    OrderLINK = table.Column<int>(nullable: true),
                    Row = table.Column<int>(nullable: false),
                    SessionLINK = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seats", x => x.LINK);
                    table.ForeignKey(
                        name: "FK_Seats_Orders_OrderLINK",
                        column: x => x.OrderLINK,
                        principalTable: "Orders",
                        principalColumn: "LINK",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Seats_Sessions_SessionLINK",
                        column: x => x.SessionLINK,
                        principalTable: "Sessions",
                        principalColumn: "LINK",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Films_AgeLimitLINK",
                table: "Films",
                column: "AgeLimitLINK");

            migrationBuilder.CreateIndex(
                name: "IX_Films_CountryLINK",
                table: "Films",
                column: "CountryLINK");

            migrationBuilder.CreateIndex(
                name: "IX_Films_DirectorLINK",
                table: "Films",
                column: "DirectorLINK");

            migrationBuilder.CreateIndex(
                name: "IX_Films_GenreLINK",
                table: "Films",
                column: "GenreLINK");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ApplicationUserId",
                table: "Orders",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_OrderLINK",
                table: "Seats",
                column: "OrderLINK");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_SessionLINK",
                table: "Seats",
                column: "SessionLINK");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_FilmLINK",
                table: "Sessions",
                column: "FilmLINK");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_HallLINK",
                table: "Sessions",
                column: "HallLINK");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Seats");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.DropTable(
                name: "Films");

            migrationBuilder.DropTable(
                name: "Halls");

            migrationBuilder.DropTable(
                name: "AgeLimits");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Directors");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_UserId",
                table: "AspNetUserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName");
        }
    }
}
