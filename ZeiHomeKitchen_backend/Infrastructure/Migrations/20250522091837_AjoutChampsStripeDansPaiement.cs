using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZeiHomeKitchen_backend.Migrations
{
    /// <inheritdoc />
    public partial class AjoutChampsStripeDansPaiement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreation",
                table: "Paiement",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateMiseAJour",
                table: "Paiement",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StripeClientSecret",
                table: "Paiement",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StripePaymentIntentId",
                table: "Paiement",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StripeStatus",
                table: "Paiement",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreation",
                table: "Paiement");

            migrationBuilder.DropColumn(
                name: "DateMiseAJour",
                table: "Paiement");

            migrationBuilder.DropColumn(
                name: "StripeClientSecret",
                table: "Paiement");

            migrationBuilder.DropColumn(
                name: "StripePaymentIntentId",
                table: "Paiement");

            migrationBuilder.DropColumn(
                name: "StripeStatus",
                table: "Paiement");
        }
    }
}
