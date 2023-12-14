using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thesis.Inventory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addconnectionid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MessageConnectionId",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MessageConnectionId",
                table: "Users");
        }
    }
}
