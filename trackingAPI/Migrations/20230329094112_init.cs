using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace trackingAPI.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Leagues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LeagueState = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leagues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Logins",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Balance = table.Column<int>(type: "int", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true, defaultValue: "User")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    dob = table.Column<DateTime>(type: "datetime2", nullable: false),
                    height_cm = table.Column<int>(type: "int", nullable: false),
                    weight_kg = table.Column<int>(type: "int", nullable: false),
                    nationality = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Overall = table.Column<int>(type: "int", nullable: false),
                    Potential = table.Column<int>(type: "int", nullable: false),
                    value_eur = table.Column<int>(type: "int", nullable: false),
                    wage_eur = table.Column<int>(type: "int", nullable: false),
                    player_positions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    preferred_foot = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    international_reputation = table.Column<int>(type: "int", nullable: false),
                    weak_foot = table.Column<int>(type: "int", nullable: false),
                    skill_moves = table.Column<int>(type: "int", nullable: false),
                    work_rate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    body_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    real_face = table.Column<int>(type: "int", nullable: false),
                    release_clause_eur = table.Column<int>(type: "int", nullable: true),
                    player_tags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    team_position = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    team_jersey_number = table.Column<int>(type: "int", nullable: true),
                    loaned_from = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    joined = table.Column<DateTime>(type: "datetime2", nullable: true),
                    contract_valid_until = table.Column<int>(type: "int", nullable: true),
                    nation_position = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    nation_jersey_number = table.Column<int>(type: "int", nullable: true),
                    Pace = table.Column<int>(type: "int", nullable: true),
                    Shooting = table.Column<int>(type: "int", nullable: true),
                    Passing = table.Column<int>(type: "int", nullable: true),
                    Dribbling = table.Column<int>(type: "int", nullable: true),
                    Defense = table.Column<int>(type: "int", nullable: true),
                    Physical = table.Column<int>(type: "int", nullable: true),
                    gk_diving = table.Column<int>(type: "int", nullable: true),
                    gk_handling = table.Column<int>(type: "int", nullable: true),
                    gk_kicking = table.Column<int>(type: "int", nullable: true),
                    gk_reflexes = table.Column<int>(type: "int", nullable: true),
                    gk_speed = table.Column<int>(type: "int", nullable: true),
                    gk_positioning = table.Column<int>(type: "int", nullable: true),
                    player_traits = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    attacking_crossing = table.Column<int>(type: "int", nullable: false),
                    attacking_finishing = table.Column<int>(type: "int", nullable: false),
                    attacking_heading_accuracy = table.Column<int>(type: "int", nullable: false),
                    attacking_short_passing = table.Column<int>(type: "int", nullable: false),
                    attacking_volleys = table.Column<int>(type: "int", nullable: false),
                    skill_dribbling = table.Column<int>(type: "int", nullable: false),
                    skill_curve = table.Column<int>(type: "int", nullable: false),
                    skill_fk_accuracy = table.Column<int>(type: "int", nullable: false),
                    skill_long_passing = table.Column<int>(type: "int", nullable: false),
                    skill_ball_control = table.Column<int>(type: "int", nullable: false),
                    movement_acceleration = table.Column<int>(type: "int", nullable: false),
                    movement_sprint_speed = table.Column<int>(type: "int", nullable: false),
                    movement_agility = table.Column<int>(type: "int", nullable: false),
                    movement_reactions = table.Column<int>(type: "int", nullable: false),
                    movement_balance = table.Column<int>(type: "int", nullable: false),
                    power_shot_power = table.Column<int>(type: "int", nullable: false),
                    power_jumping = table.Column<int>(type: "int", nullable: false),
                    power_stamina = table.Column<int>(type: "int", nullable: false),
                    power_strength = table.Column<int>(type: "int", nullable: false),
                    power_long_shots = table.Column<int>(type: "int", nullable: false),
                    mentality_aggression = table.Column<int>(type: "int", nullable: false),
                    mentality_interceptions = table.Column<int>(type: "int", nullable: false),
                    mentality_positioning = table.Column<int>(type: "int", nullable: false),
                    mentality_vision = table.Column<int>(type: "int", nullable: false),
                    mentality_penalties = table.Column<int>(type: "int", nullable: false),
                    mentality_composure = table.Column<int>(type: "int", nullable: false),
                    defending_marking = table.Column<int>(type: "int", nullable: false),
                    defending_standing_tackle = table.Column<int>(type: "int", nullable: false),
                    defending_sliding_tackle = table.Column<int>(type: "int", nullable: false),
                    goalkeeping_diving = table.Column<int>(type: "int", nullable: false),
                    goalkeeping_handling = table.Column<int>(type: "int", nullable: false),
                    goalkeeping_kicking = table.Column<int>(type: "int", nullable: false),
                    goalkeeping_positioning = table.Column<int>(type: "int", nullable: false),
                    goalkeeping_reflexes = table.Column<int>(type: "int", nullable: false),
                    ls = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    st = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    rs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lw = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lf = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cf = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    rf = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    rw = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lam = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cam = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ram = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lm = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lcm = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cm = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    rcm = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    rm = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lwb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ldm = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cdm = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    rdm = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    rwb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    lcb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    rcb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    rb = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rating = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatchState = table.Column<int>(type: "int", nullable: false),
                    DateOfMatch = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDrawAllowed = table.Column<bool>(type: "bit", nullable: false),
                    LeagueId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Round = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matches_Leagues_LeagueId",
                        column: x => x.LeagueId,
                        principalTable: "Leagues",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LeagueTeams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LeaguesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeagueTeams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeagueTeams_Leagues_LeaguesId",
                        column: x => x.LeaguesId,
                        principalTable: "Leagues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeagueTeams_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerTeams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerTeams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerTeams_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerTeams_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Team = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    BetResult = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    BetState = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bets_Logins_LoginId",
                        column: x => x.LoginId,
                        principalTable: "Logins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bets_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MatchTeams",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TeamScore = table.Column<int>(type: "int", nullable: false),
                    Result = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Seed = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchTeams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatchTeams_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MatchTeams_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Logins",
                columns: new[] { "Id", "Balance", "Email", "Password", "RefreshToken", "RefreshTokenExpiryTime", "Role", "UserName" },
                values: new object[] { new Guid("19f3e66e-b29f-471c-81f2-e23c90dd6b09"), 1000, "", "123456", null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_Bets_LoginId",
                table: "Bets",
                column: "LoginId");

            migrationBuilder.CreateIndex(
                name: "IX_Bets_MatchId",
                table: "Bets",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_LeagueTeams_LeaguesId",
                table: "LeagueTeams",
                column: "LeaguesId");

            migrationBuilder.CreateIndex(
                name: "IX_LeagueTeams_TeamId",
                table: "LeagueTeams",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Logins_UserName",
                table: "Logins",
                column: "UserName",
                unique: true,
                filter: "[UserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_LeagueId",
                table: "Matches",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchTeams_MatchId",
                table: "MatchTeams",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchTeams_TeamId",
                table: "MatchTeams",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerTeams_PlayerId",
                table: "PlayerTeams",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerTeams_TeamId",
                table: "PlayerTeams",
                column: "TeamId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bets");

            migrationBuilder.DropTable(
                name: "LeagueTeams");

            migrationBuilder.DropTable(
                name: "MatchTeams");

            migrationBuilder.DropTable(
                name: "PlayerTeams");

            migrationBuilder.DropTable(
                name: "Logins");

            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Leagues");
        }
    }
}
