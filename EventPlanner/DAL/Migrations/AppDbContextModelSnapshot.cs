﻿// <auto-generated />
using System;
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DAL.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.2");

            modelBuilder.Entity("Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AdditionalInfo")
                        .HasMaxLength(1000)
                        .HasColumnType("TEXT");

                    b.Property<string>("EventLocation")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("EventName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("EventStartTime")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("PersonalCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("WebApp.Domain.Business", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("BusinessName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("RegistryCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Businesses");
                });

            modelBuilder.Entity("WebApp.Domain.BusinessParticipant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AdditionalInfo")
                        .HasMaxLength(5000)
                        .HasColumnType("TEXT");

                    b.Property<int>("BusinessId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("EventId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ParticipantCount")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PaymentTypeId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("BusinessId");

                    b.HasIndex("EventId");

                    b.HasIndex("PaymentTypeId");

                    b.ToTable("BusinessParticipants");
                });

            modelBuilder.Entity("WebApp.Domain.PaymentType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("TypeName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("PaymentTypes");
                });

            modelBuilder.Entity("WebApp.Domain.PersonParticipant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AdditionalInfo")
                        .HasMaxLength(1500)
                        .HasColumnType("TEXT");

                    b.Property<int>("EventId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ParticipantCount")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PaymentTypeId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PersonId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.HasIndex("PaymentTypeId");

                    b.HasIndex("PersonId");

                    b.ToTable("PersonParticipants");
                });

            modelBuilder.Entity("WebApp.Domain.BusinessParticipant", b =>
                {
                    b.HasOne("WebApp.Domain.Business", "Business")
                        .WithMany("Participations")
                        .HasForeignKey("BusinessId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Event", "Event")
                        .WithMany("BusinessParticipants")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApp.Domain.PaymentType", "PaymentType")
                        .WithMany("BusinessParticipants")
                        .HasForeignKey("PaymentTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Business");

                    b.Navigation("Event");

                    b.Navigation("PaymentType");
                });

            modelBuilder.Entity("WebApp.Domain.PersonParticipant", b =>
                {
                    b.HasOne("Event", "Event")
                        .WithMany("PersonParticipants")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApp.Domain.PaymentType", "PaymentType")
                        .WithMany("PersonParticipants")
                        .HasForeignKey("PaymentTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Person", "Person")
                        .WithMany("Participations")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");

                    b.Navigation("PaymentType");

                    b.Navigation("Person");
                });

            modelBuilder.Entity("Event", b =>
                {
                    b.Navigation("BusinessParticipants");

                    b.Navigation("PersonParticipants");
                });

            modelBuilder.Entity("Person", b =>
                {
                    b.Navigation("Participations");
                });

            modelBuilder.Entity("WebApp.Domain.Business", b =>
                {
                    b.Navigation("Participations");
                });

            modelBuilder.Entity("WebApp.Domain.PaymentType", b =>
                {
                    b.Navigation("BusinessParticipants");

                    b.Navigation("PersonParticipants");
                });
#pragma warning restore 612, 618
        }
    }
}
