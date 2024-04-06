using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nozama.ProductCatalog.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Recommendations",
                columns: table => new
                {
                    RecommendationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Timestamp = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recommendations", x => x.RecommendationId);
                });

            migrationBuilder.CreateTable(
                name: "Stats",
                columns: table => new
                {
                    StatsEntryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Term = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stats", x => x.StatsEntryId);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StatsEntryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Products_Stats_StatsEntryId",
                        column: x => x.StatsEntryId,
                        principalTable: "Stats",
                        principalColumn: "StatsEntryId");
                });

            migrationBuilder.CreateTable(
                name: "ProductRecommendation",
                columns: table => new
                {
                    ProductsProductId = table.Column<int>(type: "int", nullable: false),
                    RecommendationsRecommendationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductRecommendation", x => new { x.ProductsProductId, x.RecommendationsRecommendationId });
                    table.ForeignKey(
                        name: "FK_ProductRecommendation_Products_ProductsProductId",
                        column: x => x.ProductsProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductRecommendation_Recommendations_RecommendationsRecommendationId",
                        column: x => x.RecommendationsRecommendationId,
                        principalTable: "Recommendations",
                        principalColumn: "RecommendationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductRecommendation_RecommendationsRecommendationId",
                table: "ProductRecommendation",
                column: "RecommendationsRecommendationId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_StatsEntryId",
                table: "Products",
                column: "StatsEntryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductRecommendation");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Recommendations");

            migrationBuilder.DropTable(
                name: "Stats");
        }
    }
}
