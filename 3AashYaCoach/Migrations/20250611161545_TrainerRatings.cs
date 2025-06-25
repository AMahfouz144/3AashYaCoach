using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _3AashYaCoach.Migrations
{
    /// <inheritdoc />
    public partial class TrainerRatings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrainerRatings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CoachId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RatingValue = table.Column<double>(type: "float", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainerRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainerRatings_Trainers_CoachId",
                        column: x => x.CoachId,
                        principalTable: "Trainers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrainerRatings_Users_RatedByUserId",
                        column: x => x.RatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrainerRatings_CoachId",
                table: "TrainerRatings",
                column: "CoachId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainerRatings_RatedByUserId",
                table: "TrainerRatings",
                column: "RatedByUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainerRatings");
        }
    }
}
