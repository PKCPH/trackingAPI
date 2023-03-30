using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace trackingAPI.Migrations
{
    public partial class oddsInGamematchModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Logins",
                keyColumn: "Id",
                keyValue: new Guid("3d5031fe-2efa-4a22-9a31-5f0eb820ee88"));

            migrationBuilder.AddColumn<decimal>(
                name: "Odds",
                table: "Matches",
                type: "decimal(18,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.InsertData(
                table: "Logins",
                columns: new[] { "Id", "Balance", "Email", "Password", "RefreshToken", "RefreshTokenExpiryTime", "Role", "UserName" },
                values: new object[] { new Guid("8dedad8e-b4bf-432c-a12e-8ce019ed235f"), 1000, "", "123456", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Logins",
                keyColumn: "Id",
                keyValue: new Guid("8dedad8e-b4bf-432c-a12e-8ce019ed235f"));

            migrationBuilder.DropColumn(
                name: "Odds",
                table: "Matches");

            migrationBuilder.InsertData(
                table: "Logins",
                columns: new[] { "Id", "Balance", "Email", "Password", "RefreshToken", "RefreshTokenExpiryTime", "Role", "UserName" },
                values: new object[] { new Guid("3d5031fe-2efa-4a22-9a31-5f0eb820ee88"), 1000, "", "123456", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "admin" });
        }
    }
}
