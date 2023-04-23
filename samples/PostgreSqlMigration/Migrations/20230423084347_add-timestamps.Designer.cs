﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PostgreSqlMigration;

#nullable disable

namespace PostgreSqlMigration.Migrations
{
    [DbContext(typeof(MigrationDbContext))]
    [Migration("20230423084347_add-timestamps")]
    partial class addtimestamps
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("smusdi")
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("PostgreSqlMigration.JobDao", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("UtcEndTimestamp")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("utc_end_timestamp");

                    b.Property<DateTime>("UtcStartTimestamp")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("utc_start_timestamp");

                    b.HasKey("Id")
                        .HasName("pk_jobs");

                    b.ToTable("jobs", "smusdi");
                });

            modelBuilder.Entity("Smusdi.PostgreSQL.Audit.AuditRecordDao", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ObjectId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("object_id");

                    b.Property<string>("ObjectType")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("object_type");

                    b.Property<string>("Payload")
                        .HasColumnType("text")
                        .HasColumnName("payload");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("type");

                    b.Property<string>("User")
                        .HasColumnType("text")
                        .HasColumnName("user");

                    b.Property<DateTime>("UtcCreationTimestamp")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("utc_creation_timestamp");

                    b.HasKey("Id")
                        .HasName("pk_audit_records");

                    b.HasIndex("ObjectId")
                        .HasDatabaseName("ix_audit_records_object_id");

                    b.HasIndex("ObjectType")
                        .HasDatabaseName("ix_audit_records_object_type");

                    b.ToTable("audit_records", "smusdi");
                });
#pragma warning restore 612, 618
        }
    }
}
