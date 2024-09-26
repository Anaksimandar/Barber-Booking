using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BarberBooking.Server.Migrations
{
    /// <inheritdoc />
    public partial class CreatedServiceTypeTableUsedForReadingAvaiableServicesAndMadeServiceTableOnlyForRealtionshipsWithReservations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_Reservations_ReservationId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Services");

            migrationBuilder.AlterColumn<int>(
                name: "ReservationId",
                table: "Services",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ServiceTypeId",
                table: "Services",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ServiceTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Services_ServiceTypeId",
                table: "Services",
                column: "ServiceTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Reservations_ReservationId",
                table: "Services",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_ServiceTypes_ServiceTypeId",
                table: "Services",
                column: "ServiceTypeId",
                principalTable: "ServiceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_Reservations_ReservationId",
                table: "Services");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_ServiceTypes_ServiceTypeId",
                table: "Services");

            migrationBuilder.DropTable(
                name: "ServiceTypes");

            migrationBuilder.DropIndex(
                name: "IX_Services_ServiceTypeId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "ServiceTypeId",
                table: "Services");

            migrationBuilder.AlterColumn<int>(
                name: "ReservationId",
                table: "Services",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Services",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Services",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Services",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Reservations_ReservationId",
                table: "Services",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id");
        }
    }
}
