using Microsoft.EntityFrameworkCore.Migrations;

namespace moongroup.Data.Migrations
{
    public partial class remove_string_dates_from_users_and_add_two_fields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StringCreatedAt",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "StringUpdatedAt",
                table: "AspNetUsers",
                newName: "CompanyName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CompanyName",
                table: "AspNetUsers",
                newName: "StringUpdatedAt");

            migrationBuilder.AddColumn<string>(
                name: "StringCreatedAt",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
