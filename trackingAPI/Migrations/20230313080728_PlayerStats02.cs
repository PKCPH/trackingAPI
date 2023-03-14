using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace trackingAPI.Migrations
{
    public partial class PlayerStats02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PSPercent",
                table: "Players");

            migrationBuilder.RenameColumn(
                name: "Yellow",
                table: "Players",
                newName: "Shooting");

            migrationBuilder.RenameColumn(
                name: "SpG",
                table: "Players",
                newName: "Potential");

            migrationBuilder.RenameColumn(
                name: "Red",
                table: "Players",
                newName: "Physical");

            migrationBuilder.RenameColumn(
                name: "Motm",
                table: "Players",
                newName: "Passing");

            migrationBuilder.RenameColumn(
                name: "Goals",
                table: "Players",
                newName: "Pace");

            migrationBuilder.RenameColumn(
                name: "Assists",
                table: "Players",
                newName: "Overall");

            migrationBuilder.AddColumn<int>(
                name: "Defense",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Dribbling",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Defense",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Dribbling",
                table: "Players");

            migrationBuilder.RenameColumn(
                name: "Shooting",
                table: "Players",
                newName: "Yellow");

            migrationBuilder.RenameColumn(
                name: "Potential",
                table: "Players",
                newName: "SpG");

            migrationBuilder.RenameColumn(
                name: "Physical",
                table: "Players",
                newName: "Red");

            migrationBuilder.RenameColumn(
                name: "Passing",
                table: "Players",
                newName: "Motm");

            migrationBuilder.RenameColumn(
                name: "Pace",
                table: "Players",
                newName: "Goals");

            migrationBuilder.RenameColumn(
                name: "Overall",
                table: "Players",
                newName: "Assists");

            migrationBuilder.AddColumn<decimal>(
                name: "PSPercent",
                table: "Players",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
