using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace trackingAPI.Migrations
{
    public partial class newProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Logins",
                keyColumn: "Id",
                keyValue: new Guid("1844cd12-150c-4f1e-be96-e9b0eeb9f457"));

            migrationBuilder.RenameColumn(
                name: "TimeStampForPausedMatch",
                table: "Matches",
                newName: "PlayingStateTimeStamp");

            migrationBuilder.AddColumn<int>(
                name: "PlayingState",
                table: "Matches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Logins",
                columns: new[] { "Id", "Balance", "Email", "Password", "RefreshToken", "RefreshTokenExpiryTime", "Role", "UserName" },
                values: new object[] { new Guid("314ae1e2-264f-4862-9da7-edbd389cd91e"), 1000, "", "123456", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Logins",
                keyColumn: "Id",
                keyValue: new Guid("314ae1e2-264f-4862-9da7-edbd389cd91e"));

            migrationBuilder.DropColumn(
                name: "PlayingState",
                table: "Matches");

            migrationBuilder.RenameColumn(
                name: "PlayingStateTimeStamp",
                table: "Matches",
                newName: "TimeStampForPausedMatch");

            migrationBuilder.InsertData(
                table: "Logins",
                columns: new[] { "Id", "Balance", "Email", "Password", "RefreshToken", "RefreshTokenExpiryTime", "Role", "UserName" },
                values: new object[] { new Guid("1844cd12-150c-4f1e-be96-e9b0eeb9f457"), 1000, "", "123456", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "admin" });
        }
    }
}
