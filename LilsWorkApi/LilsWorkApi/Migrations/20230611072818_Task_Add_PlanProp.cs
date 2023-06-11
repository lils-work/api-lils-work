using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LilsWorkApi.Migrations
{
    /// <inheritdoc />
    public partial class TaskAddPlanProp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlanId",
                table: "Tasks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_PlanId",
                table: "Tasks",
                column: "PlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_TaskPlans_PlanId",
                table: "Tasks",
                column: "PlanId",
                principalTable: "TaskPlans",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_TaskPlans_PlanId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_PlanId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "PlanId",
                table: "Tasks");
        }
    }
}
