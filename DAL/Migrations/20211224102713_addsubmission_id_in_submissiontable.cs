using Microsoft.EntityFrameworkCore.Migrations;

namespace moongroup.Data.Migrations
{
    public partial class addsubmission_id_in_submissiontable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SubmissionId",
                table: "ProjectFormSubmissions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectFormSubmissions_SubmissionId",
                table: "ProjectFormSubmissions",
                column: "SubmissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectFormSubmissions_ProjectFormSubmissions_SubmissionId",
                table: "ProjectFormSubmissions",
                column: "SubmissionId",
                principalTable: "ProjectFormSubmissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectFormSubmissions_ProjectFormSubmissions_SubmissionId",
                table: "ProjectFormSubmissions");

            migrationBuilder.DropIndex(
                name: "IX_ProjectFormSubmissions_SubmissionId",
                table: "ProjectFormSubmissions");

            migrationBuilder.DropColumn(
                name: "SubmissionId",
                table: "ProjectFormSubmissions");
        }
    }
}
