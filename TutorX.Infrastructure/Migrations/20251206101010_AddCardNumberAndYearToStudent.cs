using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TutorX.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCardNumberAndYearToStudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CardNumber",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "Students",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardNumber",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "Students");
        }
    }
}
