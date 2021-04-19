using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class addColorEnitiy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "Product");

            migrationBuilder.AddColumn<int>(
                name: "ColorId",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Color",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Color", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_ColorId",
                table: "Product",
                column: "ColorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Color_ColorId",
                table: "Product",
                column: "ColorId",
                principalTable: "Color",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Color_ColorId",
                table: "Product");

            migrationBuilder.DropTable(
                name: "Color");

            migrationBuilder.DropIndex(
                name: "IX_Product_ColorId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "ColorId",
                table: "Product");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
