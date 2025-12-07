using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TutorX.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDueDateToActivity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "Activities",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "Activities");
        }
    }
}
