﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using tms.Data.Context;

#nullable disable

namespace tms.Migrations
{
    [DbContext(typeof(LocalDbContext))]
    [Migration("20241205022212_Ticket2")]
    partial class Ticket2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.10");

            modelBuilder.Entity("AddOn", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<int>("AddOnType")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsSelected")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("TicketId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TotalPrice")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("TicketId");

                    b.ToTable("AddOn");
                });

            modelBuilder.Entity("DailyRevenue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateAD")
                        .HasColumnType("TEXT");

                    b.Property<int>("MonthlyRevenueId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("MonthlyRevenueId");

                    b.ToTable("DailyRevenues");
                });

            modelBuilder.Entity("MonthlyRevenue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Month")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Year")
                        .HasColumnType("INTEGER");

                    b.Property<int>("YearlyRevenueId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("YearlyRevenueId");

                    b.ToTable("MonthlyRevenues");
                });

            modelBuilder.Entity("RevenueCell", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("DailyRevenueId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("NoOfPeople")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TotalAmount")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("DailyRevenueId");

                    b.ToTable("RevenueCells");
                });

            modelBuilder.Entity("YearlyRevenue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Year")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("YearlyRevenues");
                });

            modelBuilder.Entity("tms.Data.Ticket", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<string>("BarCodeData")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("CustomText")
                        .HasColumnType("TEXT");

                    b.Property<int>("GroupSize")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsGroupVisit")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Nationality")
                        .HasColumnType("INTEGER");

                    b.Property<string>("NepaliDate")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("NoOfPeople")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PersonType")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TicketNo")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("TEXT");

                    b.Property<int>("TotalPrice")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("AddOn", b =>
                {
                    b.HasOne("tms.Data.Ticket", null)
                        .WithMany("AddOns")
                        .HasForeignKey("TicketId");
                });

            modelBuilder.Entity("DailyRevenue", b =>
                {
                    b.HasOne("MonthlyRevenue", "MonthlyRevenue")
                        .WithMany("DailyRevenues")
                        .HasForeignKey("MonthlyRevenueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("NepaliDate", "DateBS", b1 =>
                        {
                            b1.Property<int>("DailyRevenueId")
                                .HasColumnType("INTEGER");

                            b1.Property<int>("Day")
                                .HasColumnType("INTEGER");

                            b1.Property<int>("Month")
                                .HasColumnType("INTEGER");

                            b1.Property<int>("Year")
                                .HasColumnType("INTEGER");

                            b1.HasKey("DailyRevenueId");

                            b1.ToTable("DailyRevenues");

                            b1.WithOwner()
                                .HasForeignKey("DailyRevenueId");
                        });

                    b.Navigation("DateBS")
                        .IsRequired();

                    b.Navigation("MonthlyRevenue");
                });

            modelBuilder.Entity("MonthlyRevenue", b =>
                {
                    b.HasOne("YearlyRevenue", "YearlyRevenue")
                        .WithMany("MonthlyRevenues")
                        .HasForeignKey("YearlyRevenueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("YearlyRevenue");
                });

            modelBuilder.Entity("RevenueCell", b =>
                {
                    b.HasOne("DailyRevenue", "DailyRevenue")
                        .WithMany("RevenueCells")
                        .HasForeignKey("DailyRevenueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DailyRevenue");
                });

            modelBuilder.Entity("DailyRevenue", b =>
                {
                    b.Navigation("RevenueCells");
                });

            modelBuilder.Entity("MonthlyRevenue", b =>
                {
                    b.Navigation("DailyRevenues");
                });

            modelBuilder.Entity("YearlyRevenue", b =>
                {
                    b.Navigation("MonthlyRevenues");
                });

            modelBuilder.Entity("tms.Data.Ticket", b =>
                {
                    b.Navigation("AddOns");
                });
#pragma warning restore 612, 618
        }
    }
}