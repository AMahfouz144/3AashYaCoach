using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _3AashYaCoach.Migrations
{
    /// <inheritdoc />
    public partial class VideoLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VideoLink",
                table: "WorkoutExercises",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VideoLink",
                table: "WorkoutExercises");
        }
    }
}
