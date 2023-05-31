using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WA_TravelAgency_v1.Data.Migrations
{
    public partial class updateOfferTransportRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_OfferP_OfferId",
                table: "Reservation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OfferP",
                table: "OfferP");

            migrationBuilder.RenameTable(
                name: "OfferP",
                newName: "Offer");

            migrationBuilder.RenameIndex(
                name: "IX_OfferP_TransportId",
                table: "Offer",
                newName: "IX_Offer_TransportId");

            migrationBuilder.RenameIndex(
                name: "IX_OfferP_DestinationId",
                table: "Offer",
                newName: "IX_Offer_DestinationId");

            migrationBuilder.AlterColumn<Guid>(
                name: "TransportId",
                table: "Offer",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "DestinationId",
                table: "Offer",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Offer",
                table: "Offer",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Offer_Destination_DestinationId",
                table: "Offer",
                column: "DestinationId",
                principalTable: "Destination",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Offer_Transport_TransportId",
                table: "Offer",
                column: "TransportId",
                principalTable: "Transport",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OfferParameters_Offer_OfferId",
                table: "OfferParameters",
                column: "OfferId",
                principalTable: "Offer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_Offer_OfferId",
                table: "Reservation",
                column: "OfferId",
                principalTable: "Offer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offer_Destination_DestinationId",
                table: "Offer");

            migrationBuilder.DropForeignKey(
                name: "FK_Offer_Transport_TransportId",
                table: "Offer");

            migrationBuilder.DropForeignKey(
                name: "FK_OfferParameters_Offer_OfferId",
                table: "OfferParameters");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_Offer_OfferId",
                table: "Reservation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Offer",
                table: "Offer");

            migrationBuilder.RenameTable(
                name: "Offer",
                newName: "OfferP");

            migrationBuilder.RenameIndex(
                name: "IX_Offer_TransportId",
                table: "OfferP",
                newName: "IX_OfferP_TransportId");

            migrationBuilder.RenameIndex(
                name: "IX_Offer_DestinationId",
                table: "OfferP",
                newName: "IX_OfferP_DestinationId");

            migrationBuilder.AlterColumn<Guid>(
                name: "TransportId",
                table: "OfferP",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "DestinationId",
                table: "OfferP",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OfferP",
                table: "OfferP",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_OfferP_OfferId",
                table: "Reservation",
                column: "OfferId",
                principalTable: "OfferP",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
