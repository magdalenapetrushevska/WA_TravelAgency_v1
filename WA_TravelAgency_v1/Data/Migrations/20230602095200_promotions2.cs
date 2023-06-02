using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WA_TravelAgency_v1.Data.Migrations
{
    public partial class promotions2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OfferId",
                table: "Promotion",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PromotionId",
                table: "Offer",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Promotion_OfferId",
                table: "Promotion",
                column: "OfferId",
                unique: true,
                filter: "[OfferId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Promotion_Offer_OfferId",
                table: "Promotion",
                column: "OfferId",
                principalTable: "Offer",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Promotion_Offer_OfferId",
                table: "Promotion");

            migrationBuilder.DropIndex(
                name: "IX_Promotion_OfferId",
                table: "Promotion");

            migrationBuilder.DropColumn(
                name: "OfferId",
                table: "Promotion");

            migrationBuilder.DropColumn(
                name: "PromotionId",
                table: "Offer");
        }
    }
}
