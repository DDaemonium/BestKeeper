﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Server.DataAccess;

#nullable disable

namespace Server.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20220219221552_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Server.DataAccess.Models.ApplicationUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ApplicationUserRoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationUserRoleId");

                    b.ToTable("ApplicationUsers");

                    b.HasData(
                        new
                        {
                            Id = new Guid("53be8d81-2725-4c34-8d56-6ee936e76c31"),
                            ApplicationUserRoleId = new Guid("53be8d81-2725-4c34-8d56-6ee936e76c30"),
                            Email = "admin@admin.admin",
                            Name = "Admin",
                            Password = "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918",
                            PhoneNumber = "88005553535",
                            Surname = "Admin"
                        });
                });

            modelBuilder.Entity("Server.DataAccess.Models.ApplicationUserRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.ToTable("ApplicationUserRoles");

                    b.HasData(
                        new
                        {
                            Id = new Guid("53be8d81-2725-4c34-8d56-6ee936e76c30"),
                            Name = "Administrator"
                        },
                        new
                        {
                            Id = new Guid("43f991a4-5396-4f42-a741-7daa2270d054"),
                            Name = "Manager"
                        },
                        new
                        {
                            Id = new Guid("53be8d81-2725-4c34-8d56-6ee936e85c30"),
                            Name = "Waiter"
                        });
                });

            modelBuilder.Entity("Server.DataAccess.Models.Dish", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Cost")
                        .HasPrecision(7, 2)
                        .HasColumnType("decimal(7,2)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<Guid>("DishCategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("GramWeight")
                        .HasColumnType("int");

                    b.Property<string>("Ingredients")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<bool>("IsAvailabel")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("DishCategoryId");

                    b.ToTable("Dishes");
                });

            modelBuilder.Entity("Server.DataAccess.Models.DishCategory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.ToTable("DishCategories");
                });

            modelBuilder.Entity("Server.DataAccess.Models.DishOrder", b =>
                {
                    b.Property<Guid>("DishId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<int>("DishOrderStatus")
                        .HasColumnType("int");

                    b.HasKey("DishId", "OrderId");

                    b.HasIndex("OrderId");

                    b.ToTable("DishOrders");
                });

            modelBuilder.Entity("Server.DataAccess.Models.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ApplicationUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CloseDescription")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<DateTime>("Closed")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Opened")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("TableId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationUserId");

                    b.HasIndex("TableId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Server.DataAccess.Models.Table", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<bool>("IsReserved")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.ToTable("Tables");
                });

            modelBuilder.Entity("Server.DataAccess.Models.ApplicationUser", b =>
                {
                    b.HasOne("Server.DataAccess.Models.ApplicationUserRole", "ApplicationUserRole")
                        .WithMany("ApplicationUsers")
                        .HasForeignKey("ApplicationUserRoleId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("ApplicationUserRole");
                });

            modelBuilder.Entity("Server.DataAccess.Models.Dish", b =>
                {
                    b.HasOne("Server.DataAccess.Models.DishCategory", "DishCategory")
                        .WithMany("Dishes")
                        .HasForeignKey("DishCategoryId")
                        .OnDelete(DeleteBehavior.ClientNoAction)
                        .IsRequired();

                    b.Navigation("DishCategory");
                });

            modelBuilder.Entity("Server.DataAccess.Models.DishOrder", b =>
                {
                    b.HasOne("Server.DataAccess.Models.Dish", "Dish")
                        .WithMany("DishOrders")
                        .HasForeignKey("DishId")
                        .OnDelete(DeleteBehavior.ClientNoAction)
                        .IsRequired();

                    b.HasOne("Server.DataAccess.Models.Order", "Order")
                        .WithMany("DishOrders")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.ClientNoAction)
                        .IsRequired();

                    b.Navigation("Dish");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("Server.DataAccess.Models.Order", b =>
                {
                    b.HasOne("Server.DataAccess.Models.ApplicationUser", "ApplicationUser")
                        .WithMany("Orders")
                        .HasForeignKey("ApplicationUserId")
                        .OnDelete(DeleteBehavior.ClientNoAction)
                        .IsRequired();

                    b.HasOne("Server.DataAccess.Models.Table", "Table")
                        .WithMany("Orders")
                        .HasForeignKey("TableId")
                        .OnDelete(DeleteBehavior.ClientNoAction)
                        .IsRequired();

                    b.Navigation("ApplicationUser");

                    b.Navigation("Table");
                });

            modelBuilder.Entity("Server.DataAccess.Models.ApplicationUser", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("Server.DataAccess.Models.ApplicationUserRole", b =>
                {
                    b.Navigation("ApplicationUsers");
                });

            modelBuilder.Entity("Server.DataAccess.Models.Dish", b =>
                {
                    b.Navigation("DishOrders");
                });

            modelBuilder.Entity("Server.DataAccess.Models.DishCategory", b =>
                {
                    b.Navigation("Dishes");
                });

            modelBuilder.Entity("Server.DataAccess.Models.Order", b =>
                {
                    b.Navigation("DishOrders");
                });

            modelBuilder.Entity("Server.DataAccess.Models.Table", b =>
                {
                    b.Navigation("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}