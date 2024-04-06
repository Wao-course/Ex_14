using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nozama.Recommendations.Migrations
{
    /// <inheritdoc />
    public partial class latest03 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Stats_StatsEntryId",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductRecommendation_Product_ProductsProductId",
                table: "ProductRecommendation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Product",
                table: "Product");

            migrationBuilder.RenameTable(
                name: "Product",
                newName: "Products");

            migrationBuilder.RenameIndex(
                name: "IX_Product_StatsEntryId",
                table: "Products",
                newName: "IX_Products_StatsEntryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "ProductId");

            migrationBuilder.CreateTable(
                name: "Searches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Term = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timestamp = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Searches", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ProductRecommendation_Products_ProductsProductId",
                table: "ProductRecommendation",
                column: "ProductsProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Stats_StatsEntryId",
                table: "Products",
                column: "StatsEntryId",
                principalTable: "Stats",
                principalColumn: "StatsEntryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductRecommendation_Products_ProductsProductId",
                table: "ProductRecommendation");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Stats_StatsEntryId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "Searches");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "Product");

            migrationBuilder.RenameIndex(
                name: "IX_Products_StatsEntryId",
                table: "Product",
                newName: "IX_Product_StatsEntryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Product",
                table: "Product",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Stats_StatsEntryId",
                table: "Product",
                column: "StatsEntryId",
                principalTable: "Stats",
                principalColumn: "StatsEntryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductRecommendation_Product_ProductsProductId",
                table: "ProductRecommendation",
                column: "ProductsProductId",
                principalTable: "Product",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
