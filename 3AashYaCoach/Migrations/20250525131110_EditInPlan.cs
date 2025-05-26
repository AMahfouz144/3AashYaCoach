using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _3AashYaCoach.Migrations
{
    /// <inheritdoc />
    public partial class EditInPlan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "WorkoutPlans",
                newName: "PrimaryGoal");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "WorkoutPlans",
                newName: "PlanName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "WorkoutPlans",
                newName: "PlanName");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "WorkoutPlans",
                newName: "PrimaryGoal");
        }
    }
}
