using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace trackingAPI.Migrations
{
    public partial class balanceemail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Logins",
                keyColumn: "Id",
                keyValue: new Guid("66820904-e074-460f-a2c5-3994a2d80f9b"));

            migrationBuilder.AddColumn<string>(
                name: "Balance",
                table: "Logins",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Logins",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Logins",
                columns: new[] { "Id", "Balance", "Email", "Password", "RefreshToken", "RefreshTokenExpiryTime", "Role", "UserName" },
                values: new object[] { new Guid("226a25ec-5fcb-4e4c-a57c-d460cb30f58a"), "0", "", "123456", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Logins",
                keyColumn: "Id",
                keyValue: new Guid("226a25ec-5fcb-4e4c-a57c-d460cb30f58a"));

            migrationBuilder.DropColumn(
                name: "Balance",
                table: "Logins");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Logins");

            migrationBuilder.InsertData(
                table: "Logins",
                columns: new[] { "Id", "Password", "RefreshToken", "RefreshTokenExpiryTime", "Role", "UserName" },
                values: new object[] { new Guid("66820904-e074-460f-a2c5-3994a2d80f9b"), "123456", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "admin" });
        }
    }
}
