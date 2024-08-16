﻿// <auto-generated />
using System;
using FlightManagerService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FlightManagerService.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.8");

            modelBuilder.Entity("FlightManagerService.Models.Flight", b =>
                {
                    b.Property<int>("FlightId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("ArrivalTime")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DepartureTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("FlightNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("FlightId");

                    b.ToTable("Flights");
                });

            modelBuilder.Entity("FlightManagerService.Models.Passenger", b =>
                {
                    b.Property<int>("PassengerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("MiddleName")
                        .HasColumnType("TEXT");

                    b.HasKey("PassengerId");

                    b.ToTable("Passengers");
                });

            modelBuilder.Entity("FlightManagerService.Models.Ticket", b =>
                {
                    b.Property<int>("TicketId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("FlightId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PassengerId")
                        .HasColumnType("INTEGER");

                    b.HasKey("TicketId");

                    b.HasIndex("FlightId");

                    b.HasIndex("PassengerId");

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("FlightManagerService.Models.Ticket", b =>
                {
                    b.HasOne("FlightManagerService.Models.Flight", "Flight")
                        .WithMany()
                        .HasForeignKey("FlightId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FlightManagerService.Models.Passenger", "Passenger")
                        .WithMany()
                        .HasForeignKey("PassengerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Flight");

                    b.Navigation("Passenger");
                });
#pragma warning restore 612, 618
        }
    }
}
