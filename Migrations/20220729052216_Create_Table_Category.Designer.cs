﻿// <auto-generated />
using System;
using GSM.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GSM.Migrations
{
    [DbContext(typeof(GSMDbContext))]
    [Migration("20220729052216_Create_Table_Category")]
    partial class Create_Table_Category
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("GSM.Models.Category", b =>
                {
                    b.Property<int>("CategoryID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CategoryName")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("IntoMoney")
                        .HasColumnType("TEXT");

                    b.Property<double>("TotalWeight")
                        .HasColumnType("REAL");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("TEXT");

                    b.HasKey("CategoryID");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("GSM.Models.Invoice", b =>
                {
                    b.Property<int>("InvoiceID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Address")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("CustomerName")
                        .HasColumnType("TEXT");

                    b.Property<string>("InvoiceCode")
                        .HasColumnType("TEXT");

                    b.Property<string>("InvoiceNumber")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsPaid")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("TotalMoney")
                        .HasColumnType("TEXT");

                    b.HasKey("InvoiceID");

                    b.ToTable("Invoice");
                });

            modelBuilder.Entity("GSM.Models.InvoiceDetail", b =>
                {
                    b.Property<int>("InvoiceDetailID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("CraftingWages")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("GemstoneUnitPrice")
                        .HasColumnType("TEXT");

                    b.Property<double>("GemstoneWeight")
                        .HasColumnType("REAL");

                    b.Property<decimal>("GoldUnitPrice")
                        .HasColumnType("TEXT");

                    b.Property<double>("GoldWeight")
                        .HasColumnType("REAL");

                    b.Property<decimal>("IntoMoney")
                        .HasColumnType("TEXT");

                    b.Property<int?>("InvoiceID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PercentGold")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProductName")
                        .HasColumnType("TEXT");

                    b.Property<double>("SumWeight")
                        .HasColumnType("REAL");

                    b.HasKey("InvoiceDetailID");

                    b.HasIndex("InvoiceID");

                    b.ToTable("InvoiceDetails");
                });

            modelBuilder.Entity("GSM.Models.Product", b =>
                {
                    b.Property<int>("ProductID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ProductName")
                        .HasColumnType("TEXT");

                    b.HasKey("ProductID");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("GSM.Models.InvoiceDetail", b =>
                {
                    b.HasOne("GSM.Models.Invoice", "Invoice")
                        .WithMany("InvoiceDetails")
                        .HasForeignKey("InvoiceID");

                    b.Navigation("Invoice");
                });

            modelBuilder.Entity("GSM.Models.Invoice", b =>
                {
                    b.Navigation("InvoiceDetails");
                });
#pragma warning restore 612, 618
        }
    }
}
