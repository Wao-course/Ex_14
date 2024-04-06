using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nozama.Recommendations.Migrations
{
    /// <inheritdoc />
    public partial class latest04 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Stats_StatsEntryId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_StatsEntryId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "StatsEntryId",
                table: "Products");

            migrationBuilder.CreateTable(
                name: "ProductStatsEntry",
                columns: table => new
                {
                    ProductsProductId = table.Column<int>(type: "int", nullable: false),
                    StatsEntriesStatsEntryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductStatsEntry", x => new { x.ProductsProductId, x.StatsEntriesStatsEntryId });
                    table.ForeignKey(
                        name: "FK_ProductStatsEntry_Products_ProductsProductId",
                        column: x => x.ProductsProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductStatsEntry_Stats_StatsEntriesStatsEntryId",
                        column: x => x.StatsEntriesStatsEntryId,
                        principalTable: "Stats",
                        principalColumn: "StatsEntryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductStatsEntry_StatsEntriesStatsEntryId",
                table: "ProductStatsEntry",
                column: "StatsEntriesStatsEntryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductStatsEntry");

            migrationBuilder.AddColumn<int>(
                name: "StatsEntryId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_StatsEntryId",
                table: "Products",
                column: "StatsEntryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Stats_StatsEntryId",
                table: "Products",
                column: "StatsEntryId",
                principalTable: "Stats",
                principalColumn: "StatsEntryId");
        }
    }
}
