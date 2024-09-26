using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BarberBooking.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddingJoinTableServiceReservations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceTypes_Reservations_ReservationId",
                table: "ServiceTypes");

            migrationBuilder.DropIndex(
                name: "IX_ServiceTypes_ReservationId",
                table: "ServiceTypes");

            migrationBuilder.DropColumn(
                name: "ReservationId",
                table: "ServiceTypes");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReservationsServices");

            migrationBuilder.AddColumn<int>(
                name: "ReservationId",
                table: "ServiceTypes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTypes_ReservationId",
                table: "ServiceTypes",
                column: "ReservationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceTypes_Reservations_ReservationId",
                table: "ServiceTypes",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id");
        }
    }
}
