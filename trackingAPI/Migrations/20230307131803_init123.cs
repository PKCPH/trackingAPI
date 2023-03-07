using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace trackingAPI.Migrations
{
    public partial class init123 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leagues_Round_RoundsId",
                table: "Leagues");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Round_RoundId",
                table: "Matches");

            migrationBuilder.DropTable(
                name: "Round");

            migrationBuilder.DropIndex(
                name: "IX_Leagues_RoundsId",
                table: "Leagues");

            migrationBuilder.DropColumn(
                name: "RoundsId",
                table: "Leagues");

            migrationBuilder.RenameColumn(
                name: "RoundId",
                table: "Matches",
                newName: "MatchLeagueRoundsId");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_RoundId",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_MatchLeagueRounds_MatchLeagueRoundsId",
                table: "Matches");

            migrationBuilder.DropTable(
                name: "MatchLeagueRounds");

            migrationBuilder.RenameColumn(
                name: "MatchLeagueRoundsId",
                table: "Matches",
                newName: "RoundId");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_MatchLeagueRoundsId",
                table: "Matches",
                newName: "IX_Matches_RoundId");

            migrationBuilder.AddColumn<int>(
                name: "RoundsId",
                table: "Leagues",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Round",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Round", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Leagues_RoundsId",
                table: "Leagues",
                column: "RoundsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Leagues_Round_RoundsId",
                table: "Leagues",
                column: "RoundsId",
                principalTable: "Round",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Round_RoundId",
                table: "Matches",
                column: "RoundId",
                principalTable: "Round",
                principalColumn: "Id");
        }
    }
}
