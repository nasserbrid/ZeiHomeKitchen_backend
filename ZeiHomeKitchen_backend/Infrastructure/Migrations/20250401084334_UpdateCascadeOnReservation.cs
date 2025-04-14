using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZeiHomeKitchen_backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCascadeOnReservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Reservati__id_ut__4222D4EF",
                table: "Reservation");

            migrationBuilder.AddForeignKey(
                name: "FK__Reservati__id_ut__4222D4EF",
                table: "Reservation",
                column: "id_utilisateur",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Reservati__id_ut__4222D4EF",
                table: "Reservation");

            migrationBuilder.AddForeignKey(
                name: "FK__Reservati__id_ut__4222D4EF",
                table: "Reservation",
                column: "id_utilisateur",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
