using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nozama.Recommendations.Migrations
{
    /// <inheritdoc />
    public partial class latest06 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Data",
                table: "Stats");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Data",
                table: "Stats",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
