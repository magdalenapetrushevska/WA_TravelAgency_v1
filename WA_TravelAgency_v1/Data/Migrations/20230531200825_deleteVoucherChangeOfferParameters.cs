using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WA_TravelAgency_v1.Data.Migrations
{
    public partial class deleteVoucherChangeOfferParameters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Voucher");

            migrationBuilder.DropColumn(
                name: "Discount",
                table: "OfferParameters");

            migrationBuilder.RenameColumn(
                name: "МinDaysForReservation",
                table: "OfferParameters",
                newName: "МinNumOfPassengersForSureRealization");

            migrationBuilder.RenameColumn(
                name: "DiscountForChildren",
                table: "OfferParameters",
                newName: "МinNumOfPassForGratis");

            migrationBuilder.AddColumn<int>(
                name: "SureRealization",
                table: "Offer",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SureRealization",
                table: "Offer");

            migrationBuilder.RenameColumn(
                name: "МinNumOfPassengersForSureRealization",
                table: "OfferParameters",
                newName: "МinDaysForReservation");

            migrationBuilder.RenameColumn(
                name: "МinNumOfPassForGratis",
                table: "OfferParameters",
                newName: "DiscountForChildren");

            migrationBuilder.AddColumn<int>(
                name: "Discount",
                table: "OfferParameters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Voucher",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voucher", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Voucher_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Voucher_UserId",
                table: "Voucher",
                column: "UserId",
                unique: true);
        }
    }
}
