using Microsoft.EntityFrameworkCore.Migrations;

namespace moongroup.Data.Migrations
{
    public partial class orderField_infild_section_and_options : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "FormSections",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "FormFieldsOptions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "FormFields",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "FormSections");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "FormFieldsOptions");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "FormFields");
        }
    }
}
