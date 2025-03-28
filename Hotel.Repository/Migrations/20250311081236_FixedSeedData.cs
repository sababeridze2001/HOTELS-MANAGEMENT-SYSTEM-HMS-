using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Hotel.Repository.Migrations
{
    /// <inheritdoc />
    public partial class FixedSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Guests",
                columns: new[] { "Id", "MobileNumber", "Name", "PersonalNumber", "Surname" },
                values: new object[,]
                {
                    { 1, "+995599111111", "John", "98765432101", "Doe" },
                    { 2, "+995599222222", "Alice", "87654321098", "Smith" }
                });

            migrationBuilder.InsertData(
                table: "Managers",
                columns: new[] { "Id", "Email", "MobileNumber", "Name", "PersonalNumber", "Surname" },
                values: new object[] { 1, "saba@example.com", "+995599000000", "Saba", "12345678901", "Beridze" });

            migrationBuilder.InsertData(
                table: "Hotels",
                columns: new[] { "Id", "Address", "City", "Country", "ManagerId", "Name", "Rating" },
                values: new object[] { 1, "Rustaveli Avenue 10", "Tbilisi", "Georgia", 1, "Grand Hotel", 5 });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "HotelId", "IsAvailable", "Name", "Price" },
                values: new object[,]
                {
                    { 1, 1, true, "Deluxe Room", 150m },
                    { 2, 1, true, "Standard Room", 100m }
                });

            migrationBuilder.InsertData(
                table: "Reservations",
                columns: new[] { "Id", "CheckInDate", "CheckOutDate", "RoomId" },
                values: new object[] { 1, new DateTime(2024, 3, 10, 14, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 3, 15, 11, 0, 0, 0, DateTimeKind.Unspecified), 1 });

            migrationBuilder.InsertData(
                table: "GuestReservations",
                columns: new[] { "GuestId", "ReservationId" },
                values: new object[] { 1, 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "GuestReservations",
                keyColumns: new[] { "GuestId", "ReservationId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "Guests",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Guests",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Managers",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
