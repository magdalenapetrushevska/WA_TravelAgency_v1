using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WA_TravelAgency_v1.Data.Migrations
{
    public partial class addedOfferPromotions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "OfferPromotions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OfferId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Discount = table.Column<int>(type: "int", nullable: false),
                    StartDateOfPromotion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDateOfPromotion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfferPromotions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfferPromotions_Offer_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offer",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_OfferPromotions_OfferId",
                table: "OfferPromotions",
                column: "OfferId",
                unique: true,
                filter: "[OfferId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OfferPromotions");

            migrationBuilder.RenameColumn(
                name: "OfferPromotionId",
                table: "Offer",
                newName: "PromotionId");

            migrationBuilder.CreateTable(
                name: "Promotions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OfferId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Discount = table.Column<int>(type: "int", nullable: false),
                    EndDateOfPromotion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartDateOfPromotion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Promotions_Offer_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offer",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_OfferId",
                table: "Promotions",
                column: "OfferId",
                unique: true,
                filter: "[OfferId] IS NOT NULL");
        }
    }
}
