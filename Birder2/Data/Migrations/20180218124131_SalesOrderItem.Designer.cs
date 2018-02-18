﻿// <auto-generated />
using Birder2.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Birder2.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20180218124131_SalesOrderItem")]
    partial class SalesOrderItem
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Birder2.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<double>("DefaultLocationLatitude");

                    b.Property<double>("DefaultLocationLongitude");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<byte[]>("ProfileImage");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Birder2.Models.Bird", b =>
                {
                    b.Property<int>("BirdId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BritishStatusId");

                    b.Property<string>("Category");

                    b.Property<string>("Class")
                        .IsRequired();

                    b.Property<int>("ConserverationStatusId");

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("EnglishName")
                        .IsRequired();

                    b.Property<string>("Family")
                        .IsRequired();

                    b.Property<string>("Genus")
                        .IsRequired();

                    b.Property<byte[]>("Image");

                    b.Property<string>("InternationalName");

                    b.Property<DateTime>("LastUpdateDate");

                    b.Property<string>("Order")
                        .IsRequired();

                    b.Property<string>("PopulationSize");

                    b.Property<string>("Species")
                        .IsRequired();

                    b.Property<string>("ThumbnailUrl");

                    b.HasKey("BirdId");

                    b.HasIndex("BritishStatusId");

                    b.HasIndex("ConserverationStatusId");

                    b.ToTable("Bird");
                });

            modelBuilder.Entity("Birder2.Models.BritishStatus", b =>
                {
                    b.Property<int>("BritishStatusId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BirderStatusInBritain")
                        .IsRequired();

                    b.Property<string>("BtoStatusInBritain")
                        .IsRequired();

                    b.Property<DateTime>("CreationDate");

                    b.Property<DateTime>("LastUpdateDate");

                    b.HasKey("BritishStatusId");

                    b.ToTable("BritishStatus");
                });

            modelBuilder.Entity("Birder2.Models.ConserverationStatus", b =>
                {
                    b.Property<int>("ConserverationStatusId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConservationStatus")
                        .IsRequired();

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("Description");

                    b.Property<DateTime>("LastUpdateDate");

                    b.HasKey("ConserverationStatusId");

                    b.ToTable("ConservationStatus");
                });

            modelBuilder.Entity("Birder2.Models.Observation", b =>
                {
                    b.Property<int>("ObservationId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ApplicationUserId");

                    b.Property<int>("BirdId");

                    b.Property<DateTime>("CreationDate");

                    b.Property<DateTime>("LastUpdateDate");

                    b.Property<double>("LocationLatitude");

                    b.Property<double>("LocationLongitude");

                    b.Property<string>("Note");

                    b.Property<DateTime>("ObservationDateTime");

                    b.Property<int>("Quantity");

                    b.HasKey("ObservationId");

                    b.HasIndex("ApplicationUserId");

                    b.HasIndex("BirdId");

                    b.ToTable("Observation");
                });

            modelBuilder.Entity("Birder2.Models.ObservationTag", b =>
                {
                    b.Property<int>("TagId");

                    b.Property<int>("ObervationId");

                    b.HasKey("TagId", "ObervationId");

                    b.HasIndex("ObervationId");

                    b.ToTable("ObservationTag");
                });

            modelBuilder.Entity("Birder2.Models.SalesOrder", b =>
                {
                    b.Property<int>("SalesOrderId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CustomerName");

                    b.Property<string>("PONumber");

                    b.HasKey("SalesOrderId");

                    b.ToTable("SalesOrder");
                });

            modelBuilder.Entity("Birder2.Models.SalesOrderItem", b =>
                {
                    b.Property<int>("SalesOrderItemId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ProductCode");

                    b.Property<int>("Quantity");

                    b.Property<int>("SalesOrderId");

                    b.Property<decimal>("UnitPrice");

                    b.HasKey("SalesOrderItemId");

                    b.HasIndex("SalesOrderId");

                    b.ToTable("SalesOrderItems");
                });

            modelBuilder.Entity("Birder2.Models.Tag", b =>
                {
                    b.Property<int>("TagId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ApplicationUserId");

                    b.Property<string>("Description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("TagId");

                    b.HasIndex("ApplicationUserId");

                    b.ToTable("Tag");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Birder2.Models.Bird", b =>
                {
                    b.HasOne("Birder2.Models.BritishStatus", "BritishStatus")
                        .WithMany("Birds")
                        .HasForeignKey("BritishStatusId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Birder2.Models.ConserverationStatus", "BirdConserverationStatus")
                        .WithMany("Birds")
                        .HasForeignKey("ConserverationStatusId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Birder2.Models.Observation", b =>
                {
                    b.HasOne("Birder2.Models.ApplicationUser", "ApplicationUser")
                        .WithMany("Observations")
                        .HasForeignKey("ApplicationUserId");

                    b.HasOne("Birder2.Models.Bird", "Bird")
                        .WithMany("Observations")
                        .HasForeignKey("BirdId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Birder2.Models.ObservationTag", b =>
                {
                    b.HasOne("Birder2.Models.Observation", "Observation")
                        .WithMany("ObservationTags")
                        .HasForeignKey("ObervationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Birder2.Models.Tag", "Tag")
                        .WithMany("ObservationTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Birder2.Models.SalesOrderItem", b =>
                {
                    b.HasOne("Birder2.Models.SalesOrder", "SalesOrder")
                        .WithMany()
                        .HasForeignKey("SalesOrderId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Birder2.Models.Tag", b =>
                {
                    b.HasOne("Birder2.Models.ApplicationUser", "ApplicationUser")
                        .WithMany("Tags")
                        .HasForeignKey("ApplicationUserId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Birder2.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Birder2.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Birder2.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Birder2.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
