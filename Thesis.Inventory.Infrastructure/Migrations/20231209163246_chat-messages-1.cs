using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thesis.Inventory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class chatmessages1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ChatRoomId",
                table: "ChatRoomMessages",
                newName: "ChatRoomIdEntityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ChatRoomIdEntityId",
                table: "ChatRoomMessages",
                newName: "ChatRoomId");
        }
    }
}
