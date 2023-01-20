﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace trackingAPI.Migrations
{
    public partial class TeamAvailableDefaultTrue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Teams",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Teams");
        }
    }
}
