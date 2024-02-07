﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TimelineService.Api.Data;

namespace TimelineService.Api.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20210525220243_addedMentions")]
    partial class addedMentions
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.4");

            modelBuilder.Entity("TimelineService.Api.Models.Mention", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid?>("QuackId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Value")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.HasIndex("QuackId");

                    b.ToTable("Mention");
                });

            modelBuilder.Entity("TimelineService.Api.Models.Quack", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Message")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Username")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Quacks");
                });

            modelBuilder.Entity("TimelineService.Api.Models.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Username")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("UserUser", b =>
                {
                    b.Property<Guid>("FollowersUserId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("FollowingUserId")
                        .HasColumnType("char(36)");

                    b.HasKey("FollowersUserId", "FollowingUserId");

                    b.HasIndex("FollowingUserId");

                    b.ToTable("UserUser");
                });

            modelBuilder.Entity("TimelineService.Api.Models.Mention", b =>
                {
                    b.HasOne("TimelineService.Api.Models.Quack", null)
                        .WithMany("Mentions")
                        .HasForeignKey("QuackId");
                });

            modelBuilder.Entity("UserUser", b =>
                {
                    b.HasOne("TimelineService.Api.Models.User", null)
                        .WithMany()
                        .HasForeignKey("FollowersUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TimelineService.Api.Models.User", null)
                        .WithMany()
                        .HasForeignKey("FollowingUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TimelineService.Api.Models.Quack", b =>
                {
                    b.Navigation("Mentions");
                });
#pragma warning restore 612, 618
        }
    }
}
