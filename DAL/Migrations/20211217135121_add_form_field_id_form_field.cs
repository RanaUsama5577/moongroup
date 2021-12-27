using Microsoft.EntityFrameworkCore.Migrations;

namespace moongroup.Data.Migrations
{
    public partial class add_form_field_id_form_field : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FormField",
                table: "FormFields",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FormFields_FormField",
                table: "FormFields",
                column: "FormField");

            migrationBuilder.AddForeignKey(
                name: "FK_FormFields_FormFields_FormField",
                table: "FormFields",
                column: "FormField",
                principalTable: "FormFields",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormFields_FormFields_FormField",
                table: "FormFields");

            migrationBuilder.DropIndex(
                name: "IX_FormFields_FormField",
                table: "FormFields");

            migrationBuilder.DropColumn(
                name: "FormField",
                table: "FormFields");
        }
    }
}
