﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UniversityStudentTracker.API.Contexts;

#nullable disable

namespace UniversityStudentTracker.API.Migrations
{
    [DbContext(typeof(StudentPerformance))]
    [Migration("20240630161717_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("UniversityStudentTracker.API.Models.Domains.Break", b =>
                {
                    b.Property<Guid>("BreakID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWID()");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("DurationMinutes")
                        .HasColumnType("int");

                    b.Property<Guid>("UserID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("BreakID");

                    b.ToTable("Breaks");
                });

            modelBuilder.Entity("UniversityStudentTracker.API.Models.Domains.Prediction", b =>
                {
                    b.Property<Guid>("PredictionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWID()");

                    b.Property<double>("PredictedGrade")
                        .HasColumnType("float");

                    b.Property<int>("PredictedKnowledgeLevel")
                        .HasColumnType("int");

                    b.Property<DateTime>("PredictionDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("PredictionID");

                    b.ToTable("Predictions");
                });

            modelBuilder.Entity("UniversityStudentTracker.API.Models.Domains.StudySession", b =>
                {
                    b.Property<Guid>("StudySessionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWID()");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("DurationMinutes")
                        .HasColumnType("int");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("StudySessionID");

                    b.ToTable("StudySessions");
                });
#pragma warning restore 612, 618
        }
    }
}
