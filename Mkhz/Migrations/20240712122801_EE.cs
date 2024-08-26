using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mkhz.Migrations
{
    /// <inheritdoc />
    public partial class EE : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Total",
                table: "productWithQuantities",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Total",
                table: "productWithQuantities");
        }
    }
}
