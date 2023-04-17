using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace trackingAPI.Migrations
{
    public partial class changeMOdels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeagueTeams_Leagues_LeaguesId",
                table: "LeagueTeams");

            migrationBuilder.DeleteData(
                table: "Logins",
                keyColumn: "Id",
                keyValue: new Guid("0974e199-a017-4dad-bd2e-fe4c1819b1e3"));

            migrationBuilder.RenameColumn(
                name: "LeaguesId",
                table: "LeagueTeams",
                newName: "LeagueId");

            migrationBuilder.RenameIndex(
                name: "IX_LeagueTeams_LeaguesId",
                table: "LeagueTeams",
                newName: "IX_LeagueTeams_LeagueId");

            migrationBuilder.InsertData(
                table: "Logins",
                columns: new[] { "Id", "Balance", "Email", "Password", "RefreshToken", "RefreshTokenExpiryTime", "Role", "UserName" },
                values: new object[] { new Guid("048708de-d9fe-4054-891d-70e2acac18a2"), 1000, "", "123456", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "admin" });

            migrationBuilder.AddForeignKey(
                name: "FK_LeagueTeams_Leagues_LeagueId",
                table: "LeagueTeams",
                column: "LeagueId",
                principalTable: "Leagues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeagueTeams_Leagues_LeagueId",
                table: "LeagueTeams");

            migrationBuilder.DeleteData(
                table: "Logins",
                keyColumn: "Id",
                keyValue: new Guid("048708de-d9fe-4054-891d-70e2acac18a2"));

            migrationBuilder.RenameColumn(
                name: "LeagueId",
                table: "LeagueTeams",
                newName: "LeaguesId");

            migrationBuilder.RenameIndex(
                name: "IX_LeagueTeams_LeagueId",
                table: "LeagueTeams",
                newName: "IX_LeagueTeams_LeaguesId");

            migrationBuilder.InsertData(
                table: "Logins",
                columns: new[] { "Id", "Balance", "Email", "Password", "RefreshToken", "RefreshTokenExpiryTime", "Role", "UserName" },
                values: new object[] { new Guid("0974e199-a017-4dad-bd2e-fe4c1819b1e3"), 1000, "", "123456", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "admin" });

            migrationBuilder.AddForeignKey(
                name: "FK_LeagueTeams_Leagues_LeaguesId",
                table: "LeagueTeams",
                column: "LeaguesId",
                principalTable: "Leagues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
