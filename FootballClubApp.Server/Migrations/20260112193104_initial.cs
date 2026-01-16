using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FootballClubApp.Server.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clubs",
                columns: table => new
                {
                    ClubId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClubCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ClubName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FoundedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StadiumName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ClubLogo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clubs", x => x.ClubId);
                });

            migrationBuilder.CreateTable(
                name: "Positions",
                columns: table => new
                {
                    PositionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PositionName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.PositionId);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    PlayerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PlayerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Nationality = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    JerseyNumber = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    PlayerPhoto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClubId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.PlayerId);
                    table.ForeignKey(
                        name: "FK_Players_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "ClubId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerDetails",
                columns: table => new
                {
                    PlayerDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    PositionId = table.Column<int>(type: "int", nullable: false),
                    ContractStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ContractEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AnnualSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GoalsScored = table.Column<int>(type: "int", nullable: false),
                    Assists = table.Column<int>(type: "int", nullable: false),
                    MatchesPlayed = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerDetails", x => x.PlayerDetailId);
                    table.ForeignKey(
                        name: "FK_PlayerDetails_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerDetails_Positions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Positions",
                        principalColumn: "PositionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Clubs",
                columns: new[] { "ClubId", "ClubCode", "ClubLogo", "ClubName", "FoundedDate", "IsActive", "StadiumName" },
                values: new object[,]
                {
                    { 1, "MU-2025", "/images/clubs/manchester-united.png", "Manchester United", new DateTime(1878, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Old Trafford" },
                    { 2, "FCB-2025", "/images/clubs/barcelona.png", "FC Barcelona", new DateTime(1899, 11, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Camp Nou" },
                    { 3, "RM-2025", "/images/clubs/real-madrid.png", "Real Madrid", new DateTime(1902, 3, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Santiago Bernabéu" }
                });

            migrationBuilder.InsertData(
                table: "Positions",
                columns: new[] { "PositionId", "Description", "PositionName" },
                values: new object[,]
                {
                    { 1, "Defends the goal", "Goalkeeper (GK)" },
                    { 2, "Central defender", "Defender (CB)" },
                    { 3, "Left side defender", "Left Back (LB)" },
                    { 4, "Right side defender", "Right Back (RB)" },
                    { 5, "Central midfielder", "Midfielder (CM)" },
                    { 6, "Creative playmaker", "Attacking Midfielder (CAM)" },
                    { 7, "Side attacker", "Winger (LW/RW)" },
                    { 8, "Forward, main goal scorer", "Striker (ST)" }
                });

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "PlayerId", "ClubId", "DateOfBirth", "IsActive", "JerseyNumber", "Nationality", "PlayerCode", "PlayerName", "PlayerPhoto" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(1985, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 7, "Portugal", "CR7-2025", "Cristiano Ronaldo", "/images/players/ronaldo.jpg" },
                    { 2, 2, new DateTime(1987, 6, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 10, "Argentina", "LM10-2025", "Lionel Messi", "/images/players/messi.jpg" },
                    { 3, 2, new DateTime(1992, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), true, 11, "Brazil", "NJR11-2025", "Neymar Jr", "/images/players/neymar.jpg" }
                });

            migrationBuilder.InsertData(
                table: "PlayerDetails",
                columns: new[] { "PlayerDetailId", "AnnualSalary", "Assists", "ContractEnd", "ContractStart", "GoalsScored", "MatchesPlayed", "PlayerId", "PositionId" },
                values: new object[,]
                {
                    { 1, 50000000m, 45, new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 145, 200, 1, 8 },
                    { 2, 55000000m, 305, new DateTime(2025, 6, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 672, 778, 2, 6 },
                    { 3, 45000000m, 237, new DateTime(2027, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 436, 649, 3, 7 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerDetails_PlayerId",
                table: "PlayerDetails",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerDetails_PositionId",
                table: "PlayerDetails",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_ClubId",
                table: "Players",
                column: "ClubId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerDetails");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Positions");

            migrationBuilder.DropTable(
                name: "Clubs");
        }
    }
}
