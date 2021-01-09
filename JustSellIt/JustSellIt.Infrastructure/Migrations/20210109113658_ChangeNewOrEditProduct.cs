using Microsoft.EntityFrameworkCore.Migrations;

namespace JustSellIt.Infrastructure.Migrations
{
    public partial class ChangeNewOrEditProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsNegotiate",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsNew",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsNegotiate",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsNew",
                table: "Products");
        }
    }
}
