using Microsoft.EntityFrameworkCore.Migrations;

namespace InventoryBeginners.Migrations
{
    public partial class photocvh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhotoUrl",
                table: "Candidats",
                newName: "CoverImageUrl");

            migrationBuilder.AddColumn<string>(
                name: "BookPdfUrl",
                table: "Candidats",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookPdfUrl",
                table: "Candidats");

            migrationBuilder.RenameColumn(
                name: "CoverImageUrl",
                table: "Candidats",
                newName: "PhotoUrl");
        }
    }
}
