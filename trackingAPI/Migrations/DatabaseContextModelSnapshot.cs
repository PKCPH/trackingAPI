﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using trackingAPI.Data;

#nullable disable

namespace trackingAPI.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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
                            Id = new Guid("bde74eda-899e-44e0-a39e-2d9342c49254"),
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

                    b.Property<int?>("Round")
                        .HasColumnType("int");

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

                    b.Property<int>("Defense")
                        .HasColumnType("int");

                    b.Property<int>("Dribbling")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Overall")
                        .HasColumnType("int");

                    b.Property<int>("Pace")
                        .HasColumnType("int");

                    b.Property<int>("Passing")
                        .HasColumnType("int");

                    b.Property<int>("Physical")
                        .HasColumnType("int");

                    b.Property<int>("Potential")
                        .HasColumnType("int");

                    b.Property<int>("Shooting")
                        .HasColumnType("int");

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
                    b.HasOne("trackingAPI.Models.League", null)
                        .WithMany("Gamematches")
                        .HasForeignKey("LeagueId");
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
