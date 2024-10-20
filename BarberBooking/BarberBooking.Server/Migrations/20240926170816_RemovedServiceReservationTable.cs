using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BarberBooking.Server.Migrations
{
    /// <inheritdoc />
    public partial class RemovedServiceReservationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<int>(
                name: "ServiceTypeId",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_ServiceTypeId",
                table: "Reservations",
                column: "ServiceTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_ServiceTypes_ServiceTypeId",
                table: "Reservations",
                column: "ServiceTypeId",
                principalTable: "ServiceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_ServiceTypes_ServiceTypeId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_ServiceTypeId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "ServiceTypeId",
                table: "Reservations");

            migrationBuilder.CreateTable(
                name: "ReservationsServices",
                columns: table => new
                {
                    ServiceReservationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReservationId = table.Column<int>(type: "int", nullable: false),
                    ServiceTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationsServices", x => x.ServiceReservationId);
                    table.ForeignKey(
                        name: "FK_ReservationsServices_Reservations_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "Reservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReservationsServices_ServiceTypes_ServiceTypeId",
                        column: x => x.ServiceTypeId,
                        principalTable: "ServiceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReservationsServices_ReservationId",
                table: "ReservationsServices",
                column: "ReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationsServices_ServiceTypeId",
                table: "ReservationsServices",
                column: "ServiceTypeId");
        }
    }
}
