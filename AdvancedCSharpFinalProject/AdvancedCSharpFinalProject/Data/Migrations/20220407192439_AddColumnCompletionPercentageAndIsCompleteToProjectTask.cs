using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdvancedCSharpFinalProject.Data.Migrations
{
    public partial class AddColumnCompletionPercentageAndIsCompleteToProjectTask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTask_AspNetUsers_DeveloperId",
                table: "ProjectTask");

            migrationBuilder.AlterColumn<string>(
                name: "DeveloperId",
                table: "ProjectTask",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "CompletionPercentage",
                table: "ProjectTask",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "ProjectTask",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTask_AspNetUsers_DeveloperId",
                table: "ProjectTask",
                column: "DeveloperId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTask_AspNetUsers_DeveloperId",
                table: "ProjectTask");

            migrationBuilder.DropColumn(
                name: "CompletionPercentage",
                table: "ProjectTask");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "ProjectTask");

            migrationBuilder.AlterColumn<string>(
                name: "DeveloperId",
                table: "ProjectTask",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTask_AspNetUsers_DeveloperId",
                table: "ProjectTask",
                column: "DeveloperId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
