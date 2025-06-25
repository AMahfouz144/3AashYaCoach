using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _3AashYaCoach.Migrations
{
    /// <inheritdoc />
    public partial class ModifyInPlanTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFollwoed",
                table: "WorkoutPlans",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "WorkoutExercises",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFollwoed",
                table: "WorkoutPlans");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "WorkoutExercises");
        }
    }
}
