﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using trackingAPI.Data;

#nullable disable

namespace trackingAPI.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20230330060029_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("trackingAPI.Models.Bet", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<int>("BetResult")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<int>("BetState")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<Guid>("LoginId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("MatchId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Team")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("LoginId");

                    b.HasIndex("MatchId");

                    b.ToTable("Bets");
                });

            modelBuilder.Entity("trackingAPI.Models.Gamematch", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateOfMatch")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDrawAllowed")
                        .HasColumnType("bit");

                    b.Property<Guid?>("LeagueId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("MatchState")
                        .HasColumnType("int");

                    b.Property<int?>("Round")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LeagueId");

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("trackingAPI.Models.League", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("LeagueState")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Leagues");
                });

            modelBuilder.Entity("trackingAPI.Models.LeagueTeam", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("LeaguesId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TeamId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("LeaguesId");

                    b.HasIndex("TeamId");

                    b.ToTable("LeagueTeams");
                });

            modelBuilder.Entity("trackingAPI.Models.Login", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("Balance")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RefreshTokenExpiryTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Role")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("User");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserName")
                        .IsUnique()
                        .HasFilter("[UserName] IS NOT NULL");

                    b.ToTable("Logins");

                    b.HasData(
                        new
                        {
                            Id = new Guid("e0e32e76-ee05-427b-b520-311375832480"),
                            Balance = 1000,
                            Email = "",
                            Password = "123456",
                            RefreshTokenExpiryTime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Role = "Admin",
                            UserName = "admin"
                        });
                });

            modelBuilder.Entity("trackingAPI.Models.MatchTeam", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("MatchId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Result")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<int?>("Seed")
                        .HasColumnType("int");

                    b.Property<Guid?>("TeamId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("TeamScore")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MatchId");

                    b.HasIndex("TeamId");

                    b.ToTable("MatchTeams");
                });

            modelBuilder.Entity("trackingAPI.Models.Player", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<int?>("Defense")
                        .HasColumnType("int");

                    b.Property<int?>("Dribbling")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Overall")
                        .HasColumnType("int");

                    b.Property<int?>("Pace")
                        .HasColumnType("int");

                    b.Property<int?>("Passing")
                        .HasColumnType("int");

                    b.Property<int?>("Physical")
                        .HasColumnType("int");

                    b.Property<int>("Potential")
                        .HasColumnType("int");

                    b.Property<int?>("Shooting")
                        .HasColumnType("int");

                    b.Property<int>("attacking_crossing")
                        .HasColumnType("int");

                    b.Property<int>("attacking_finishing")
                        .HasColumnType("int");

                    b.Property<int>("attacking_heading_accuracy")
                        .HasColumnType("int");

                    b.Property<int>("attacking_short_passing")
                        .HasColumnType("int");

                    b.Property<int>("attacking_volleys")
                        .HasColumnType("int");

                    b.Property<string>("body_type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("cam")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("cb")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("cdm")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("cf")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("cm")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("contract_valid_until")
                        .HasColumnType("int");

                    b.Property<int>("defending_marking")
                        .HasColumnType("int");

                    b.Property<int>("defending_sliding_tackle")
                        .HasColumnType("int");

                    b.Property<int>("defending_standing_tackle")
                        .HasColumnType("int");

                    b.Property<DateTime>("dob")
                        .HasColumnType("datetime2");

                    b.Property<int?>("gk_diving")
                        .HasColumnType("int");

                    b.Property<int?>("gk_handling")
                        .HasColumnType("int");

                    b.Property<int?>("gk_kicking")
                        .HasColumnType("int");

                    b.Property<int?>("gk_positioning")
                        .HasColumnType("int");

                    b.Property<int?>("gk_reflexes")
                        .HasColumnType("int");

                    b.Property<int?>("gk_speed")
                        .HasColumnType("int");

                    b.Property<int>("goalkeeping_diving")
                        .HasColumnType("int");

                    b.Property<int>("goalkeeping_handling")
                        .HasColumnType("int");

                    b.Property<int>("goalkeeping_kicking")
                        .HasColumnType("int");

                    b.Property<int>("goalkeeping_positioning")
                        .HasColumnType("int");

                    b.Property<int>("goalkeeping_reflexes")
                        .HasColumnType("int");

                    b.Property<int>("height_cm")
                        .HasColumnType("int");

                    b.Property<int>("international_reputation")
                        .HasColumnType("int");

                    b.Property<DateTime?>("joined")
                        .HasColumnType("datetime2");

                    b.Property<string>("lam")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("lb")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("lcb")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("lcm")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ldm")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("lf")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("lm")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("loaned_from")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ls")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("lw")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("lwb")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("mentality_aggression")
                        .HasColumnType("int");

                    b.Property<int>("mentality_composure")
                        .HasColumnType("int");

                    b.Property<int>("mentality_interceptions")
                        .HasColumnType("int");

                    b.Property<int>("mentality_penalties")
                        .HasColumnType("int");

                    b.Property<int>("mentality_positioning")
                        .HasColumnType("int");

                    b.Property<int>("mentality_vision")
                        .HasColumnType("int");

                    b.Property<int>("movement_acceleration")
                        .HasColumnType("int");

                    b.Property<int>("movement_agility")
                        .HasColumnType("int");

                    b.Property<int>("movement_balance")
                        .HasColumnType("int");

                    b.Property<int>("movement_reactions")
                        .HasColumnType("int");

                    b.Property<int>("movement_sprint_speed")
                        .HasColumnType("int");

                    b.Property<int?>("nation_jersey_number")
                        .HasColumnType("int");

                    b.Property<string>("nation_position")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("nationality")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("player_positions")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("player_tags")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("player_traits")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("power_jumping")
                        .HasColumnType("int");

                    b.Property<int>("power_long_shots")
                        .HasColumnType("int");

                    b.Property<int>("power_shot_power")
                        .HasColumnType("int");

                    b.Property<int>("power_stamina")
                        .HasColumnType("int");

                    b.Property<int>("power_strength")
                        .HasColumnType("int");

                    b.Property<string>("preferred_foot")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ram")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("rb")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("rcb")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("rcm")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("rdm")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("real_face")
                        .HasColumnType("int");

                    b.Property<int?>("release_clause_eur")
                        .HasColumnType("int");

                    b.Property<string>("rf")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("rm")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("rs")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("rw")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("rwb")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("skill_ball_control")
                        .HasColumnType("int");

                    b.Property<int>("skill_curve")
                        .HasColumnType("int");

                    b.Property<int>("skill_dribbling")
                        .HasColumnType("int");

                    b.Property<int>("skill_fk_accuracy")
                        .HasColumnType("int");

                    b.Property<int>("skill_long_passing")
                        .HasColumnType("int");

                    b.Property<int>("skill_moves")
                        .HasColumnType("int");

                    b.Property<string>("st")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("team_jersey_number")
                        .HasColumnType("int");

                    b.Property<string>("team_position")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("value_eur")
                        .HasColumnType("int");

                    b.Property<int>("wage_eur")
                        .HasColumnType("int");

                    b.Property<int>("weak_foot")
                        .HasColumnType("int");

                    b.Property<int>("weight_kg")
                        .HasColumnType("int");

                    b.Property<string>("work_rate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("trackingAPI.Models.PlayerTeam", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PlayerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TeamId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.HasIndex("TeamId");

                    b.ToTable("PlayerTeams");
                });

            modelBuilder.Entity("trackingAPI.Models.Team", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool?>("IsAvailable")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("Rating")
                        .HasColumnType("decimal(18,4)");

                    b.HasKey("Id");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("trackingAPI.Models.Bet", b =>
                {
                    b.HasOne("trackingAPI.Models.Login", null)
                        .WithMany("Bets")
                        .HasForeignKey("LoginId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("trackingAPI.Models.Gamematch", "Match")
                        .WithMany("Bets")
                        .HasForeignKey("MatchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Match");
                });

            modelBuilder.Entity("trackingAPI.Models.Gamematch", b =>
                {
                    b.HasOne("trackingAPI.Models.League", "league")
                        .WithMany("Gamematches")
                        .HasForeignKey("LeagueId");

                    b.Navigation("league");
                });

            modelBuilder.Entity("trackingAPI.Models.LeagueTeam", b =>
                {
                    b.HasOne("trackingAPI.Models.League", "Leagues")
                        .WithMany("Teams")
                        .HasForeignKey("LeaguesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("trackingAPI.Models.Team", "Team")
                        .WithMany("Leagues")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Leagues");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("trackingAPI.Models.MatchTeam", b =>
                {
                    b.HasOne("trackingAPI.Models.Gamematch", "Match")
                        .WithMany("ParticipatingTeams")
                        .HasForeignKey("MatchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("trackingAPI.Models.Team", "Team")
                        .WithMany("Matches")
                        .HasForeignKey("TeamId");

                    b.Navigation("Match");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("trackingAPI.Models.PlayerTeam", b =>
                {
                    b.HasOne("trackingAPI.Models.Player", null)
                        .WithMany("Teams")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("trackingAPI.Models.Team", null)
                        .WithMany("Players")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("trackingAPI.Models.Gamematch", b =>
                {
                    b.Navigation("Bets");

                    b.Navigation("ParticipatingTeams");
                });

            modelBuilder.Entity("trackingAPI.Models.League", b =>
                {
                    b.Navigation("Gamematches");

                    b.Navigation("Teams");
                });

            modelBuilder.Entity("trackingAPI.Models.Login", b =>
                {
                    b.Navigation("Bets");
                });

            modelBuilder.Entity("trackingAPI.Models.Player", b =>
                {
                    b.Navigation("Teams");
                });

            modelBuilder.Entity("trackingAPI.Models.Team", b =>
                {
                    b.Navigation("Leagues");

                    b.Navigation("Matches");

                    b.Navigation("Players");
                });
#pragma warning restore 612, 618
        }
    }
}