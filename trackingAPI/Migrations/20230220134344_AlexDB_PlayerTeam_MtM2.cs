using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace trackingAPI.Migrations
{
    public partial class AlexDB_PlayerTeam_MtM2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerTeam_Teams_TeamId",
                table: "PlayerTeam");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerTeam",
                table: "PlayerTeam");

            migrationBuilder.RenameTable(
                name: "PlayerTeam",
                newName: "PlayerTeams");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerTeam_TeamId",
                table: "PlayerTeams",
                newName: "IX_PlayerTeams_TeamId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerTeams",
                table: "PlayerTeams",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerTeams_Teams_TeamId",
                table: "PlayerTeams",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerTeams_Teams_TeamId",
                table: "PlayerTeams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerTeams",
                table: "PlayerTeams");

            migrationBuilder.RenameTable(
                name: "PlayerTeams",
                newName: "PlayerTeam");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerTeams_TeamId",
                table: "PlayerTeam",
                newName: "IX_PlayerTeam_TeamId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerTeam",
                table: "PlayerTeam",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerTeam_Teams_TeamId",
                table: "PlayerTeam",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
