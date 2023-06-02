using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WA_TravelAgency_v1.Data.Migrations
{
    public partial class addedImageForOfferAndPromotion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "Promotion",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "Offer",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "Promotion");

            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "Offer");
        }
    }
}
