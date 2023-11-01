using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel.Migrations
{
    public partial class removeBookingCartFromRoom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingCartRoom");

            migrationBuilder.AddColumn<Guid>(
                name: "BookingCartId",
                table: "Rooms",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_BookingCartId",
                table: "Rooms",
                column: "BookingCartId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_BookingCarts_BookingCartId",
                table: "Rooms",
                column: "BookingCartId",
                principalTable: "BookingCarts",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_BookingCarts_BookingCartId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_BookingCartId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "BookingCartId",
                table: "Rooms");

            migrationBuilder.CreateTable(
                name: "BookingCartRoom",
                columns: table => new
                {
                    BookingCartsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SelectedRoomsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingCartRoom", x => new { x.BookingCartsId, x.SelectedRoomsId });
                    table.ForeignKey(
                        name: "FK_BookingCartRoom_BookingCarts_BookingCartsId",
                        column: x => x.BookingCartsId,
                        principalTable: "BookingCarts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingCartRoom_Rooms_SelectedRoomsId",
                        column: x => x.SelectedRoomsId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingCartRoom_SelectedRoomsId",
                table: "BookingCartRoom",
                column: "SelectedRoomsId");
        }
    }
}
