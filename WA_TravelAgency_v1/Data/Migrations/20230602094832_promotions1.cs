using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WA_TravelAgency_v1.Data.Migrations
{
    public partial class promotions1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Promotion",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Discount = table.Column<int>(type: "int", nullable: false),
                    StartDateOfPromotion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDateOfPromotion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotion", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Promotion");
        }
    }
}
