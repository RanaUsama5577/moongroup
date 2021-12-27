using Microsoft.EntityFrameworkCore.Migrations;

namespace moongroup.Data.Migrations
{
    public partial class addsubmission_id_in_submissiontable_nullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectFormSubmissions_ProjectFormSubmissions_SubmissionId",
                table: "ProjectFormSubmissions");

            migrationBuilder.AlterColumn<int>(
                name: "SubmissionId",
                table: "ProjectFormSubmissions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectFormSubmissions_ProjectFormSubmissions_SubmissionId",
                table: "ProjectFormSubmissions",
                column: "SubmissionId",
                principalTable: "ProjectFormSubmissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectFormSubmissions_ProjectFormSubmissions_SubmissionId",
                table: "ProjectFormSubmissions");

            migrationBuilder.AlterColumn<int>(
                name: "SubmissionId",
                table: "ProjectFormSubmissions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectFormSubmissions_ProjectFormSubmissions_SubmissionId",
                table: "ProjectFormSubmissions",
                column: "SubmissionId",
                principalTable: "ProjectFormSubmissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
