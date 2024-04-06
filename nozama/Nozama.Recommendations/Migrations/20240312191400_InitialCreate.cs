using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nozama.Recommendations.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatsEntryId",
                table: "Product",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Stats",
                columns: table => new
                {
                    StatsEntryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Term = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stats", x => x.StatsEntryId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_StatsEntryId",
                table: "Product",
                column: "StatsEntryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Stats_StatsEntryId",
                table: "Product",
                column: "StatsEntryId",
                principalTable: "Stats",
                principalColumn: "StatsEntryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Stats_StatsEntryId",
                table: "Product");

            migrationBuilder.DropTable(
                name: "Stats");

            migrationBuilder.DropIndex(
                name: "IX_Product_StatsEntryId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "StatsEntryId",
                table: "Product");
        }
    }
}
