using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thesis.Inventory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateimagetype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageType",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageType",
                table: "Products");
        }
    }
}
