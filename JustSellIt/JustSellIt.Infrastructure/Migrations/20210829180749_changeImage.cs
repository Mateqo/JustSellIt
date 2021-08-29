using Microsoft.EntityFrameworkCore.Migrations;

namespace JustSellIt.Infrastructure.Migrations
{
    public partial class changeImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MainImageUrl",
                table: "Products",
                newName: "MainImageName");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "Images",
                newName: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MainImageName",
                table: "Products",
                newName: "MainImageUrl");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Images",
                newName: "Url");
        }
    }
}
