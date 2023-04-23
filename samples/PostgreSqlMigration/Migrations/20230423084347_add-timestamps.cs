using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PostgreSqlMigration.Migrations
{
    /// <inheritdoc />
    public partial class addtimestamps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "utc_end_timestamp",
                schema: "smusdi",
                table: "jobs",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "utc_start_timestamp",
                schema: "smusdi",
                table: "jobs",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "utc_end_timestamp",
                schema: "smusdi",
                table: "jobs");

            migrationBuilder.DropColumn(
                name: "utc_start_timestamp",
                schema: "smusdi",
                table: "jobs");
        }
    }
}
