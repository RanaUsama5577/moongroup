using Microsoft.EntityFrameworkCore.Migrations;

namespace moongroup.Data.Migrations
{
    public partial class projects_documents_in_database_setting_id : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectDocuments_ProjectsSettings_ProjectId",
                table: "ProjectDocuments");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "ProjectDocuments",
                newName: "ProjectSettingId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectDocuments_ProjectId",
                table: "ProjectDocuments",
                newName: "IX_ProjectDocuments_ProjectSettingId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectDocuments_ProjectsSettings_ProjectSettingId",
                table: "ProjectDocuments",
                column: "ProjectSettingId",
                principalTable: "ProjectsSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectDocuments_ProjectsSettings_ProjectSettingId",
                table: "ProjectDocuments");

            migrationBuilder.RenameColumn(
                name: "ProjectSettingId",
                table: "ProjectDocuments",
                newName: "ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectDocuments_ProjectSettingId",
                table: "ProjectDocuments",
                newName: "IX_ProjectDocuments_ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectDocuments_ProjectsSettings_ProjectId",
                table: "ProjectDocuments",
                column: "ProjectId",
                principalTable: "ProjectsSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
