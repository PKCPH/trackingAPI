using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace trackingAPI.Migrations
{
    public partial class _2nd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bets_Matches_MatchId",
                table: "Bets");

            migrationBuilder.DeleteData(
                table: "Logins",
                keyColumn: "Id",
                keyValue: new Guid("d3e4e1f2-7fd3-4037-9bb9-490350cf157e"));

            migrationBuilder.AlterColumn<Guid>(
                name: "MatchId",
                table: "Bets",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.InsertData(
                table: "Logins",
                columns: new[] { "Id", "Balance", "Email", "Password", "RefreshToken", "RefreshTokenExpiryTime", "Role", "UserName" },
                values: new object[] { new Guid("5f6e5e46-703f-433b-a994-748f0feaa18b"), 1000, "", "123456", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "admin" });

            migrationBuilder.AddForeignKey(
                name: "FK_Bets_Matches_MatchId",
                table: "Bets",
                column: "MatchId",
                principalTable: "Matches",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bets_Matches_MatchId",
                table: "Bets");

            migrationBuilder.DeleteData(
                table: "Logins",
                keyColumn: "Id",
                keyValue: new Guid("5f6e5e46-703f-433b-a994-748f0feaa18b"));

            migrationBuilder.AlterColumn<Guid>(
                name: "MatchId",
                table: "Bets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Logins",
                columns: new[] { "Id", "Balance", "Email", "Password", "RefreshToken", "RefreshTokenExpiryTime", "Role", "UserName" },
                values: new object[] { new Guid("d3e4e1f2-7fd3-4037-9bb9-490350cf157e"), 1000, "", "123456", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "admin" });

            migrationBuilder.AddForeignKey(
                name: "FK_Bets_Matches_MatchId",
                table: "Bets",
                column: "MatchId",
                principalTable: "Matches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
