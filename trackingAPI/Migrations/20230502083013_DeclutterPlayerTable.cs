using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace trackingAPI.Migrations
{
    public partial class DeclutterPlayerTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Defense",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Dribbling",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Pace",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Passing",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Physical",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Potential",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Shooting",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "attacking_crossing",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "attacking_finishing",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "attacking_heading_accuracy",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "attacking_short_passing",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "attacking_volleys",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "body_type",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "cam",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "cb",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "cdm",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "cf",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "cm",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "contract_valid_until",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "defending_marking",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "defending_sliding_tackle",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "defending_standing_tackle",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "gk_diving",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "gk_handling",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "gk_kicking",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "gk_positioning",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "gk_reflexes",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "gk_speed",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "goalkeeping_diving",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "goalkeeping_handling",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "goalkeeping_kicking",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "goalkeeping_positioning",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "goalkeeping_reflexes",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "international_reputation",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "joined",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "lam",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "lb",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "lcb",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "lcm",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "ldm",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "lf",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "lm",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "loaned_from",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "ls",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "lw",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "lwb",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "mentality_aggression",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "mentality_composure",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "mentality_interceptions",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "mentality_penalties",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "mentality_positioning",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "mentality_vision",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "movement_acceleration",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "movement_agility",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "movement_balance",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "movement_reactions",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "movement_sprint_speed",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "nation_jersey_number",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "nation_position",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "player_tags",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "player_traits",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "power_jumping",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "power_long_shots",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "power_shot_power",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "power_stamina",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "power_strength",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "ram",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "rb",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "rcb",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "rcm",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "rdm",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "real_face",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "release_clause_eur",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "rf",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "rm",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "rs",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "rw",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "rwb",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "skill_ball_control",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "skill_curve",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "skill_dribbling",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "skill_fk_accuracy",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "skill_long_passing",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "skill_moves",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "st",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "team_jersey_number",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "team_position",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "value_eur",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "wage_eur",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "weak_foot",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "work_rate",
                table: "Players");

            migrationBuilder.AlterColumn<string>(
                name: "preferred_foot",
                table: "Players",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "player_positions",
                table: "Players",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "preferred_foot",
                table: "Players",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "player_positions",
                table: "Players",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Defense",
                table: "Players",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Dribbling",
                table: "Players",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Pace",
                table: "Players",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Passing",
                table: "Players",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Physical",
                table: "Players",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Potential",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Shooting",
                table: "Players",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "attacking_crossing",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "attacking_finishing",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "attacking_heading_accuracy",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "attacking_short_passing",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "attacking_volleys",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "body_type",
                table: "Players",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "cam",
                table: "Players",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "cb",
                table: "Players",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "cdm",
                table: "Players",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "cf",
                table: "Players",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "cm",
                table: "Players",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "contract_valid_until",
                table: "Players",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "defending_marking",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "defending_sliding_tackle",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "defending_standing_tackle",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "gk_diving",
                table: "Players",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "gk_handling",
                table: "Players",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "gk_kicking",
                table: "Players",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "gk_positioning",
                table: "Players",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "gk_reflexes",
                table: "Players",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "gk_speed",
                table: "Players",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "goalkeeping_diving",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "goalkeeping_handling",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "goalkeeping_kicking",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "goalkeeping_positioning",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "goalkeeping_reflexes",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "international_reputation",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "joined",
                table: "Players",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "lam",
                table: "Players",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "lb",
                table: "Players",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "lcb",
                table: "Players",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "lcm",
                table: "Players",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ldm",
                table: "Players",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "lf",
                table: "Players",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "lm",
                table: "Players",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "loaned_from",
                table: "Players",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ls",
                table: "Players",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "lw",
                table: "Players",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "lwb",
                table: "Players",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "mentality_aggression",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "mentality_composure",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "mentality_interceptions",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "mentality_penalties",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "mentality_positioning",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "mentality_vision",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "movement_acceleration",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "movement_agility",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "movement_balance",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "movement_reactions",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "movement_sprint_speed",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nation_jersey_number",
                table: "Players",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "nation_position",
                table: "Players",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "player_tags",
                table: "Players",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "player_traits",
                table: "Players",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "power_jumping",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "power_long_shots",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "power_shot_power",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "power_stamina",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "power_strength",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ram",
                table: "Players",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "rb",
                table: "Players",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "rcb",
                table: "Players",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "rcm",
                table: "Players",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "rdm",
                table: "Players",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "real_face",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "release_clause_eur",
                table: "Players",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "rf",
                table: "Players",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "rm",
                table: "Players",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "rs",
                table: "Players",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "rw",
                table: "Players",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "rwb",
                table: "Players",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "skill_ball_control",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "skill_curve",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "skill_dribbling",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "skill_fk_accuracy",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "skill_long_passing",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "skill_moves",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "st",
                table: "Players",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "team_jersey_number",
                table: "Players",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "team_position",
                table: "Players",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "value_eur",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "wage_eur",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "weak_foot",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "work_rate",
                table: "Players",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
