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

            modelBuilder.Entity("trackingAPI.Models.Gamematch", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateOfMatch")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDrawAllowed")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<bool>("IsLeagueGame")
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

                    b.Property<bool?>("InTournament")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<Guid>("LeaguesId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TeamId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("TeamSeed")
                        .HasColumnType("int");

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

                    b.Property<string>("Name")
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

                    b.HasKey("Id");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("trackingAPI.Models.Gamematch", b =>
                {
                    b.HasOne("trackingAPI.Models.League", "League")
                        .WithMany("Gamematches")
                        .HasForeignKey("LeagueId");

                    b.Navigation("League");
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
                    b.Navigation("ParticipatingTeams");
                });

            modelBuilder.Entity("trackingAPI.Models.League", b =>
                {
                    b.Navigation("Gamematches");

                    b.Navigation("Teams");
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
