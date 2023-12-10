using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thesis.Inventory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class chatmessages2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatRoomMessages_ChatRooms_ChatRoomEntityId",
                table: "ChatRoomMessages");

            migrationBuilder.DropColumn(
                name: "ChatRoomIdEntityId",
                table: "ChatRoomMessages");

            migrationBuilder.AlterColumn<int>(
                name: "ChatRoomEntityId",
                table: "ChatRoomMessages",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatRoomMessages_ChatRooms_ChatRoomEntityId",
                table: "ChatRoomMessages",
                column: "ChatRoomEntityId",
                principalTable: "ChatRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatRoomMessages_ChatRooms_ChatRoomEntityId",
                table: "ChatRoomMessages");

            migrationBuilder.AlterColumn<int>(
                name: "ChatRoomEntityId",
                table: "ChatRoomMessages",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ChatRoomIdEntityId",
                table: "ChatRoomMessages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatRoomMessages_ChatRooms_ChatRoomEntityId",
                table: "ChatRoomMessages",
                column: "ChatRoomEntityId",
                principalTable: "ChatRooms",
                principalColumn: "Id");
        }
    }
}
