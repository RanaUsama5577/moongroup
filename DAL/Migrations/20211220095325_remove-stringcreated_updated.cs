using Microsoft.EntityFrameworkCore.Migrations;

namespace moongroup.Data.Migrations
{
    public partial class removestringcreated_updated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StringCreatedAt",
                table: "FormSections");

            migrationBuilder.DropColumn(
                name: "StringUpdatedAt",
                table: "FormSections");

            migrationBuilder.DropColumn(
                name: "StringCreatedAt",
                table: "FormFieldsOptions");

            migrationBuilder.DropColumn(
                name: "StringUpdatedAt",
                table: "FormFieldsOptions");

            migrationBuilder.DropColumn(
                name: "StringCreatedAt",
                table: "FormFields");

            migrationBuilder.DropColumn(
                name: "StringUpdatedAt",
                table: "FormFields");

            migrationBuilder.DropColumn(
                name: "StringCreatedAt",
                table: "FormFieldOptionBinders");

            migrationBuilder.DropColumn(
                name: "StringUpdatedAt",
                table: "FormFieldOptionBinders");

            migrationBuilder.DropColumn(
                name: "StringCreatedAt",
                table: "ClientApplications");

            migrationBuilder.DropColumn(
                name: "StringUpdatedAt",
                table: "ClientApplications");

            migrationBuilder.DropColumn(
                name: "StringCreatedAt",
                table: "ApplicationSettings");

            migrationBuilder.DropColumn(
                name: "StringUpdatedAt",
                table: "ApplicationSettings");

            migrationBuilder.DropColumn(
                name: "StringCreatedAt",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "StringUpdatedAt",
                table: "Applications");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StringCreatedAt",
                table: "FormSections",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StringUpdatedAt",
                table: "FormSections",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StringCreatedAt",
                table: "FormFieldsOptions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StringUpdatedAt",
                table: "FormFieldsOptions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StringCreatedAt",
                table: "FormFields",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StringUpdatedAt",
                table: "FormFields",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StringCreatedAt",
                table: "FormFieldOptionBinders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StringUpdatedAt",
                table: "FormFieldOptionBinders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StringCreatedAt",
                table: "ClientApplications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StringUpdatedAt",
                table: "ClientApplications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StringCreatedAt",
                table: "ApplicationSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StringUpdatedAt",
                table: "ApplicationSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StringCreatedAt",
                table: "Applications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StringUpdatedAt",
                table: "Applications",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
