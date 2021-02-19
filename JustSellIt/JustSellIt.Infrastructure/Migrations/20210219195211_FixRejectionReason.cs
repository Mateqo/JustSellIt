using Microsoft.EntityFrameworkCore.Migrations;

namespace JustSellIt.Infrastructure.Migrations
{
    public partial class FixRejectionReason : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_RejectionReasons_RejectionReasonId1",
                table: "Products");

            migrationBuilder.DropTable(
                name: "RejectionReasons");

            migrationBuilder.DropIndex(
                name: "IX_Products_RejectionReasonId1",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "RejectionReasonId1",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "RejectionReasonId",
                table: "Products",
                newName: "RejectionReason");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RejectionReason",
                table: "Products",
                newName: "RejectionReasonId");

            migrationBuilder.AddColumn<int>(
                name: "RejectionReasonId1",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RejectionReasons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RejectionReasons", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_RejectionReasonId1",
                table: "Products",
                column: "RejectionReasonId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_RejectionReasons_RejectionReasonId1",
                table: "Products",
                column: "RejectionReasonId1",
                principalTable: "RejectionReasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
