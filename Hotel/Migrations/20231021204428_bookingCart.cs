using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel.Migrations
{
    public partial class bookingCart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BookingCartId",
                table: "Rooms",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BookingCarts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CheckIn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CheckOut = table.Column<DateTime>(type: "datetime2", nullable: false),
                    People = table.Column<int>(type: "int", nullable: false),
                    RoomCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingCarts", x => x.Id);
                });

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

            migrationBuilder.DropTable(
                name: "BookingCarts");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_BookingCartId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "BookingCartId",
                table: "Rooms");
        }
    }
}
