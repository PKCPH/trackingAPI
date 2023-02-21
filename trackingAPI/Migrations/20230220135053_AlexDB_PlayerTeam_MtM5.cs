using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace trackingAPI.Migrations
{
    public partial class AlexDB_PlayerTeam_MtM5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_playerTeams_Players_PlayerId",
                table: "playerTeams");

            migrationBuilder.DropForeignKey(
                name: "FK_playerTeams_Teams_TeamId",
                table: "playerTeams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_playerTeams",
                table: "playerTeams");

            migrationBuilder.RenameTable(
                name: "playerTeams",
                newName: "PlayerTeams");

            migrationBuilder.RenameIndex(
                name: "IX_playerTeams_TeamId",
                table: "PlayerTeams",
                newName: "IX_PlayerTeams_TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_playerTeams_PlayerId",
                table: "PlayerTeams",
                newName: "IX_PlayerTeams_PlayerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerTeams",
                table: "PlayerTeams",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerTeams_Players_PlayerId",
                table: "PlayerTeams",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_PlayerTeams_Players_PlayerId",
                table: "PlayerTeams");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerTeams_Teams_TeamId",
                table: "PlayerTeams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerTeams",
                table: "PlayerTeams");

            migrationBuilder.RenameTable(
                name: "PlayerTeams",
                newName: "playerTeams");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerTeams_TeamId",
                table: "playerTeams",
                newName: "IX_playerTeams_TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerTeams_PlayerId",
                table: "playerTeams",
                newName: "IX_playerTeams_PlayerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_playerTeams",
                table: "playerTeams",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_playerTeams_Players_PlayerId",
                table: "playerTeams",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_playerTeams_Teams_TeamId",
                table: "playerTeams",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
