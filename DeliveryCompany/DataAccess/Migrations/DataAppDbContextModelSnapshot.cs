﻿// <auto-generated />
using System;
using DeliveryCompany.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataAccess.Migrations
{
    [DbContext(typeof(DataAppDbContext))]
    partial class DataAppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DeliveryCompany.Models.DbModels.City", b =>
                {
                    b.Property<int>("CityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnOrder(1);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CityId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CityId");

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("DeliveryCompany.Models.DbModels.DeliveryCarOrder", b =>
                {
                    b.Property<int>("DeliveryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnOrder(1);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DeliveryId"));

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("DeliveryCarId")
                        .HasColumnType("int")
                        .HasColumnOrder(2);

                    b.Property<int>("DeliveryStatus")
                        .HasColumnType("int");

                    b.Property<int>("OrderId")
                        .HasColumnType("int")
                        .HasColumnOrder(3);

                    b.HasKey("DeliveryId");

                    b.HasIndex("DeliveryCarId");

                    b.HasIndex("OrderId");

                    b.ToTable("DeliveryCarOrders");
                });

            modelBuilder.Entity("DeliveryCompany.Models.DbModels.DeliveryCars", b =>
                {
                    b.Property<int>("DeliveryCarsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DeliveryCarsId"));

                    b.Property<int>("AssigmentStatus")
                        .HasColumnType("int");

                    b.Property<int>("CityId")
                        .HasColumnType("int");

                    b.Property<int>("DeliveryCarStatus")
                        .HasColumnType("int");

                    b.Property<int?>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<int>("MaxHeight")
                        .HasColumnType("int");

                    b.Property<int>("MaxLength")
                        .HasColumnType("int");

                    b.Property<int>("MaxWeight")
                        .HasColumnType("int");

                    b.Property<int>("MaxWidth")
                        .HasColumnType("int");

                    b.HasKey("DeliveryCarsId");

                    b.HasIndex("CityId");

                    b.HasIndex("EmployeeId")
                        .IsUnique()
                        .HasFilter("[EmployeeId] IS NOT NULL");

                    b.ToTable("DeliveryCars");
                });

            modelBuilder.Entity("DeliveryCompany.Models.DbModels.Employee", b =>
                {
                    b.Property<int>("EmployeeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EmployeeId"));

                    b.Property<int>("AssigmentStatus")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("EmployeeId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("DeliveryCompany.Models.DbModels.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderId"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CityId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("OrderStatus")
                        .HasColumnType("int");

                    b.Property<int>("PackagesId")
                        .HasColumnType("int");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("OrderId");

                    b.HasIndex("PackagesId")
                        .IsUnique();

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("DeliveryCompany.Models.DbModels.Packages", b =>
                {
                    b.Property<int>("PackagesId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PackagesId"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Height")
                        .HasColumnType("int");

                    b.Property<int>("Length")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Weight")
                        .HasColumnType("int");

                    b.Property<int>("Width")
                        .HasColumnType("int");

                    b.HasKey("PackagesId");

                    b.ToTable("Packages");
                });

            modelBuilder.Entity("DeliveryCompany.Models.DbModels.PageDescriptions", b =>
                {
                    b.Property<int>("PageDescriptionsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PageDescriptionsId"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PageDescriptionsId");

                    b.ToTable("PageDescriptions");

                    b.HasData(
                        new
                        {
                            PageDescriptionsId = 1,
                            Description = "initial description"
                        });
                });

            modelBuilder.Entity("DeliveryCompany.Models.DbModels.DeliveryCarOrder", b =>
                {
                    b.HasOne("DeliveryCompany.Models.DbModels.DeliveryCars", "DeliveryCars")
                        .WithMany()
                        .HasForeignKey("DeliveryCarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DeliveryCompany.Models.DbModels.Order", "Order")
                        .WithMany()
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DeliveryCars");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("DeliveryCompany.Models.DbModels.DeliveryCars", b =>
                {
                    b.HasOne("DeliveryCompany.Models.DbModels.City", "City")
                        .WithMany("DeliveryCarsInCity")
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DeliveryCompany.Models.DbModels.Employee", "Employee")
                        .WithOne("DeliveryCars")
                        .HasForeignKey("DeliveryCompany.Models.DbModels.DeliveryCars", "EmployeeId");

                    b.Navigation("City");

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("DeliveryCompany.Models.DbModels.Order", b =>
                {
                    b.HasOne("DeliveryCompany.Models.DbModels.Packages", "Packages")
                        .WithOne("Order")
                        .HasForeignKey("DeliveryCompany.Models.DbModels.Order", "PackagesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Packages");
                });

            modelBuilder.Entity("DeliveryCompany.Models.DbModels.City", b =>
                {
                    b.Navigation("DeliveryCarsInCity");
                });

            modelBuilder.Entity("DeliveryCompany.Models.DbModels.Employee", b =>
                {
                    b.Navigation("DeliveryCars")
                        .IsRequired();
                });

            modelBuilder.Entity("DeliveryCompany.Models.DbModels.Packages", b =>
                {
                    b.Navigation("Order")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
