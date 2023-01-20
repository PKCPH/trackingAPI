using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace trackingAPI.Migrations
{
    public partial class removeNameTeam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MatchTeam_Teams_TeamsId",
                table: "MatchTeam");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Teams");

            migrationBuilder.RenameColumn(
                name: "TeamsId",
                table: "MatchTeam",
                newName: "ParticipatingTeamsId");

            migrationBuilder.RenameIndex(
                name: "IX_MatchTeam_TeamsId",
                table: "MatchTeam",
                newName: "IX_MatchTeam_ParticipatingTeamsId");

            migrationBuilder.AddForeignKey(
                name: "FK_MatchTeam_Teams_ParticipatingTeamsId",
                table: "MatchTeam",
                column: "ParticipatingTeamsId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MatchTeam_Teams_ParticipatingTeamsId",
                table: "MatchTeam");

            migrationBuilder.RenameColumn(
                name: "ParticipatingTeamsId",
                table: "MatchTeam",
                newName: "TeamsId");

            migrationBuilder.RenameIndex(
                name: "IX_MatchTeam_ParticipatingTeamsId",
                table: "MatchTeam",
                newName: "IX_MatchTeam_TeamsId");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Teams",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_MatchTeam_Teams_TeamsId",
                table: "MatchTeam",
                column: "TeamsId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
