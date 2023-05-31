using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WA_TravelAgency_v1.Data.Migrations
{
    public partial class addedOffersRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Offer",
                table: "Offer");

            migrationBuilder.RenameTable(
                name: "Offer",
                newName: "OfferP");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OfferP",
                table: "OfferP",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Reservation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OfferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AmountToPay = table.Column<int>(type: "int", nullable: false),
                    ReservationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Paid = table.Column<int>(type: "int", nullable: false),
                    AmountPaid = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservation_OfferP_OfferId",
                        column: x => x.OfferId,
                        principalTable: "OfferP",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OfferParameters_OfferId",
                table: "OfferParameters",
                column: "OfferId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OfferP_DestinationId",
                table: "OfferP",
                column: "DestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_OfferP_TransportId",
                table: "OfferP",
                column: "TransportId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_OfferId",
                table: "Reservation",
                column: "OfferId");

            migrationBuilder.AddForeignKey(
                name: "FK_OfferP_Destination_DestinationId",
                table: "OfferP",
                column: "DestinationId",
                principalTable: "Destination",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OfferP_Transport_TransportId",
                table: "OfferP",
                column: "TransportId",
                principalTable: "Transport",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OfferParameters_OfferP_OfferId",
                table: "OfferParameters",
                column: "OfferId",
                principalTable: "OfferP",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfferP_Destination_DestinationId",
                table: "OfferP");

            migrationBuilder.DropForeignKey(
                name: "FK_OfferP_Transport_TransportId",
                table: "OfferP");

            migrationBuilder.DropForeignKey(
                name: "FK_OfferParameters_OfferP_OfferId",
                table: "OfferParameters");

            migrationBuilder.DropTable(
                name: "Reservation");

            migrationBuilder.DropIndex(
                name: "IX_OfferParameters_OfferId",
                table: "OfferParameters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OfferP",
                table: "OfferP");

            migrationBuilder.DropIndex(
                name: "IX_OfferP_DestinationId",
                table: "OfferP");

            migrationBuilder.DropIndex(
                name: "IX_OfferP_TransportId",
                table: "OfferP");

            migrationBuilder.RenameTable(
                name: "OfferP",
                newName: "Offer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Offer",
                table: "Offer",
                column: "Id");
        }
    }
}
