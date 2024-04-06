using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nozama.Recommendations.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.ProductId);
                });

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
                        name: "FK_ProductRecommendation_Product_ProductsProductId",
                        column: x => x.ProductsProductId,
                        principalTable: "Product",
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductRecommendation");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Recommendations");
        }
    }
}
