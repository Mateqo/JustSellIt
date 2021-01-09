using Microsoft.EntityFrameworkCore.Migrations;

namespace JustSellIt.Infrastructure.Migrations
{
    public partial class AddLocationAndPhoneToNewOrEditProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneContact",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PhoneContact",
                table: "Products");
        }
    }
}
