using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TutorX.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ActivityStudentManyToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActivityStudentMembership",
                columns: table => new
                {
                    ActivityId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityStudentMembership", x => new { x.ActivityId, x.StudentId });
                    table.ForeignKey(
                        name: "FK_ActivityStudentMembership_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActivityStudentMembership_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityStudentMembership_StudentId",
                table: "ActivityStudentMembership",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityStudentMembership");
        }
    }
}
