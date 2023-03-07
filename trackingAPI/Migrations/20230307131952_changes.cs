using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace trackingAPI.Migrations
{
    public partial class changes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_MatchLeagueRounds_MatchLeagueRoundsId",
                table: "Matches");

            migrationBuilder.DropTable(
                name: "MatchLeagueRounds");

            migrationBuilder.RenameColumn(
                name: "MatchLeagueRoundsId",
                table: "Matches",
                newName: "LeagueGamematchRoundId");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_MatchLeagueRoundsId",
                table: "Matches",
                newName: "IX_Matches_LeagueGamematchRoundId");

            migrationBuilder.CreateTable(
                name: "LeagueGamematchRound",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeagueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeagueGamematchRound", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeagueGamematchRound_Leagues_LeagueId",
                        column: x => x.LeagueId,
                        principalTable: "Leagues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LeagueGamematchRound_LeagueId",
                table: "LeagueGamematchRound",
                column: "LeagueId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_LeagueGamematchRound_LeagueGamematchRoundId",
                table: "Matches",
                column: "LeagueGamematchRoundId",
                principalTable: "LeagueGamematchRound",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_LeagueGamematchRound_LeagueGamematchRoundId",
                table: "Matches");

            migrationBuilder.DropTable(
                name: "LeagueGamematchRound");

            migrationBuilder.RenameColumn(
                name: "LeagueGamematchRoundId",
                table: "Matches",
                newName: "MatchLeagueRoundsId");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_LeagueGamematchRoundId",
                table: "Matches",
                newName: "IX_Matches_MatchLeagueRoundsId");

            migrationBuilder.CreateTable(
                name: "MatchLeagueRounds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeagueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchLeagueRounds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatchLeagueRounds_Leagues_LeagueId",
                        column: x => x.LeagueId,
                        principalTable: "Leagues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MatchLeagueRounds_LeagueId",
                table: "MatchLeagueRounds",
                column: "LeagueId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_MatchLeagueRounds_MatchLeagueRoundsId",
                table: "Matches",
                column: "MatchLeagueRoundsId",
                principalTable: "MatchLeagueRounds",
                principalColumn: "Id");
        }
    }
}
