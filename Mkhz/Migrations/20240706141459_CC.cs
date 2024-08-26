using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mkhz.Migrations
{
    /// <inheritdoc />
    public partial class CC : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "confirmed",
                table: "productWithQuantities",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "confirmed",
                table: "productWithQuantities");
        }
    }
}
