using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace trackingAPI.Migrations
{
    public partial class init234 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MatchTeams_Leagues_LeagueId",
                table: "MatchTeams");

            migrationBuilder.DropForeignKey(
                name: "FK_MatchTeams_Teams_TeamId",
                table: "MatchTeams");

            migrationBuilder.AlterColumn<Guid>(
                name: "TeamId",
                table: "MatchTeams",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "LeagueId",
                table: "MatchTeams",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_MatchTeams_Leagues_LeagueId",
                table: "MatchTeams",
                column: "LeagueId",
                principalTable: "Leagues",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MatchTeams_Teams_TeamId",
                table: "MatchTeams",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MatchTeams_Leagues_LeagueId",
                table: "MatchTeams");

            migrationBuilder.DropForeignKey(
                name: "FK_MatchTeams_Teams_TeamId",
                table: "MatchTeams");

            migrationBuilder.AlterColumn<Guid>(
                name: "TeamId",
                table: "MatchTeams",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "LeagueId",
                table: "MatchTeams",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MatchTeams_Leagues_LeagueId",
                table: "MatchTeams",
                column: "LeagueId",
                principalTable: "Leagues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MatchTeams_Teams_TeamId",
                table: "MatchTeams",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id");
        }
    }
}
