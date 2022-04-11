using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdvancedCSharpFinalProject.Data.Migrations
{
    public partial class ChangeProjectManagerAndDeveloperToApplicationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Project_AspNetUsers_DeveloperId",
                table: "Project");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTask_AspNetUsers_ProjectManagerId",
                table: "ProjectTask");

            migrationBuilder.DropIndex(
                name: "IX_ProjectTask_ProjectManagerId",
                table: "ProjectTask");

            migrationBuilder.DropIndex(
                name: "IX_Project_DeveloperId",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "ProjectManagerId",
                table: "ProjectTask");

            migrationBuilder.DropColumn(
                name: "DeveloperId",
                table: "Project");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProjectManagerId",
                table: "ProjectTask",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeveloperId",
                table: "Project",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTask_ProjectManagerId",
                table: "ProjectTask",
                column: "ProjectManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_DeveloperId",
                table: "Project",
                column: "DeveloperId");

            migrationBuilder.AddForeignKey(
                name: "FK_Project_AspNetUsers_DeveloperId",
                table: "Project",
                column: "DeveloperId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTask_AspNetUsers_ProjectManagerId",
                table: "ProjectTask",
                column: "ProjectManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
