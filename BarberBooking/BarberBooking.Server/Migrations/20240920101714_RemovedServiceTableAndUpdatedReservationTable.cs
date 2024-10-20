using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BarberBooking.Server.Migrations
{
    /// <inheritdoc />
    public partial class RemovedServiceTableAndUpdatedReservationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Services");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReservationId = table.Column<int>(type: "int", nullable: false),
                    ServiceTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Services_Reservations_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "Reservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Services_ServiceTypes_ServiceTypeId",
                        column: x => x.ServiceTypeId,
                        principalTable: "ServiceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Services_ReservationId",
                table: "Services",
                column: "ReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_ServiceTypeId",
                table: "Services",
                column: "ServiceTypeId");
        }
    }
}
