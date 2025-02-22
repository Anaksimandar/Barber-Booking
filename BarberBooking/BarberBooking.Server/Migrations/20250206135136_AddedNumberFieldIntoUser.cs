using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BarberBooking.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddedNumberFieldIntoUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Number",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Number",
                table: "Users");
        }
    }
}
