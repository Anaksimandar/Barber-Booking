using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BarberBooking.Server.Migrations
{
    /// <inheritdoc />
    public partial class ChangedColumnNameInReservationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Reservations",
                newName: "DateOfReservation");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateOfReservation",
                table: "Reservations",
                newName: "CreatedAt");
        }
    }
}
