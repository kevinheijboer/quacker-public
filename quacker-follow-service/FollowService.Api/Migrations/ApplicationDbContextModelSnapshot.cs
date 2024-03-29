﻿// <auto-generated />
using System;
using FollowService.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FollowService.Api.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.4");

            modelBuilder.Entity("FollowService.Api.Models.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

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

            modelBuilder.Entity("UserUser", b =>
                {
                    b.HasOne("FollowService.Api.Models.User", null)
                        .WithMany()
                        .HasForeignKey("FollowersUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FollowService.Api.Models.User", null)
                        .WithMany()
                        .HasForeignKey("FollowingUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
