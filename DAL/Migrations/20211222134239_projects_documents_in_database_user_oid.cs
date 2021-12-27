using Microsoft.EntityFrameworkCore.Migrations;

namespace moongroup.Data.Migrations
{
    public partial class projects_documents_in_database_user_oid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ProjectDocuments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectDocuments_UserId",
                table: "ProjectDocuments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectDocuments_AspNetUsers_UserId",
                table: "ProjectDocuments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectDocuments_AspNetUsers_UserId",
                table: "ProjectDocuments");

            migrationBuilder.DropIndex(
                name: "IX_ProjectDocuments_UserId",
                table: "ProjectDocuments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ProjectDocuments");
        }
    }
}
