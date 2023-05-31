using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WA_TravelAgency_v1.Data.Migrations
{
    public partial class addedVoucherUserRel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Voucher",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Voucher_UserId",
                table: "Voucher",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Voucher_AspNetUsers_UserId",
                table: "Voucher",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Voucher_AspNetUsers_UserId",
                table: "Voucher");

            migrationBuilder.DropIndex(
                name: "IX_Voucher_UserId",
                table: "Voucher");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Voucher");
        }
    }
}
