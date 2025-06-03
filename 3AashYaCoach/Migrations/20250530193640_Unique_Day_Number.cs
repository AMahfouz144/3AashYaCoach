using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _3AashYaCoach.Migrations
{
    /// <inheritdoc />
    public partial class Unique_Day_Number : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WorkoutDays_WorkoutPlanId",
                table: "WorkoutDays");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutDays_WorkoutPlanId_DayNumber",
                table: "WorkoutDays",
                columns: new[] { "WorkoutPlanId", "DayNumber" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WorkoutDays_WorkoutPlanId_DayNumber",
                table: "WorkoutDays");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutDays_WorkoutPlanId",
                table: "WorkoutDays",
                column: "WorkoutPlanId");
        }
    }
}
