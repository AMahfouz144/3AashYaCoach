using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _3AashYaCoach.Migrations
{
    /// <inheritdoc />
    public partial class edit_subscrition_plan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SubscriptionId",
                table: "PlanSubscriptions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_PlanSubscriptions_SubscriptionId",
                table: "PlanSubscriptions",
                column: "SubscriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanSubscriptions_Subscriptions_SubscriptionId",
                table: "PlanSubscriptions",
                column: "SubscriptionId",
                principalTable: "Subscriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlanSubscriptions_Subscriptions_SubscriptionId",
                table: "PlanSubscriptions");

            migrationBuilder.DropIndex(
                name: "IX_PlanSubscriptions_SubscriptionId",
                table: "PlanSubscriptions");

            migrationBuilder.DropColumn(
                name: "SubscriptionId",
                table: "PlanSubscriptions");
        }
    }
}
