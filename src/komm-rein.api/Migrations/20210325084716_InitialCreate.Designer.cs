﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using komm_rein.model;

namespace komm_rein.api.Migrations
{
    [DbContext(typeof(KraDbContext))]
    [Migration("20210325084716_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("komm_rein.model.Facility", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CreatedBySid")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("DeletedBySid")
                        .HasColumnType("text");

                    b.Property<DateTime?>("DeleteddDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("OwnerSid")
                        .HasColumnType("text");

                    b.Property<Guid?>("SettingsID")
                        .HasColumnType("uuid");

                    b.Property<string>("UpdatedBySid")
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("ID");

                    b.HasIndex("SettingsID");

                    b.ToTable("Facilities");
                });

            modelBuilder.Entity("komm_rein.model.FacilitySettings", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("CountingMode")
                        .HasColumnType("integer");

                    b.Property<string>("CreatedBySid")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("DeletedBySid")
                        .HasColumnType("text");

                    b.Property<DateTime?>("DeleteddDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("MaxNumberofVisitors")
                        .HasColumnType("integer");

                    b.Property<string>("OwnerSid")
                        .HasColumnType("text");

                    b.Property<TimeSpan>("SlotSize")
                        .HasColumnType("interval");

                    b.Property<double>("SlotStatusThreshold")
                        .HasColumnType("double precision");

                    b.Property<string>("UpdatedBySid")
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("ID");

                    b.ToTable("FacilitySettings");
                });

            modelBuilder.Entity("komm_rein.model.Household", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CreatedBySid")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("DeletedBySid")
                        .HasColumnType("text");

                    b.Property<DateTime?>("DeleteddDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("NumberOfChildren")
                        .HasColumnType("integer");

                    b.Property<int>("NumberOfPersons")
                        .HasColumnType("integer");

                    b.Property<string>("OwnerSid")
                        .HasColumnType("text");

                    b.Property<string>("UpdatedBySid")
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid?>("VisitID")
                        .HasColumnType("uuid");

                    b.HasKey("ID");

                    b.HasIndex("VisitID");

                    b.ToTable("Households");
                });

            modelBuilder.Entity("komm_rein.model.OpeningHours", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CreatedBySid")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("DayOfWeek")
                        .HasColumnType("integer");

                    b.Property<string>("DeletedBySid")
                        .HasColumnType("text");

                    b.Property<DateTime?>("DeleteddDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid?>("FacilityID")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("From")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("OwnerSid")
                        .HasColumnType("text");

                    b.Property<DateTime>("To")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("UpdatedBySid")
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("ID");

                    b.HasIndex("FacilityID");

                    b.ToTable("OpeningHours");
                });

            modelBuilder.Entity("komm_rein.model.Visit", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CreatedBySid")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("DeletedBySid")
                        .HasColumnType("text");

                    b.Property<DateTime?>("DeleteddDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid?>("FacilityID")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("From")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("OwnerSid")
                        .HasColumnType("text");

                    b.Property<DateTime>("To")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("UpdatedBySid")
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("ID");

                    b.HasIndex("FacilityID");

                    b.ToTable("Visits");
                });

            modelBuilder.Entity("komm_rein.model.Facility", b =>
                {
                    b.HasOne("komm_rein.model.FacilitySettings", "Settings")
                        .WithMany()
                        .HasForeignKey("SettingsID");

                    b.Navigation("Settings");
                });

            modelBuilder.Entity("komm_rein.model.Household", b =>
                {
                    b.HasOne("komm_rein.model.Visit", null)
                        .WithMany("Households")
                        .HasForeignKey("VisitID");
                });

            modelBuilder.Entity("komm_rein.model.OpeningHours", b =>
                {
                    b.HasOne("komm_rein.model.Facility", null)
                        .WithMany("OpeningHours")
                        .HasForeignKey("FacilityID");
                });

            modelBuilder.Entity("komm_rein.model.Visit", b =>
                {
                    b.HasOne("komm_rein.model.Facility", "Facility")
                        .WithMany()
                        .HasForeignKey("FacilityID");

                    b.Navigation("Facility");
                });

            modelBuilder.Entity("komm_rein.model.Facility", b =>
                {
                    b.Navigation("OpeningHours");
                });

            modelBuilder.Entity("komm_rein.model.Visit", b =>
                {
                    b.Navigation("Households");
                });
#pragma warning restore 612, 618
        }
    }
}
