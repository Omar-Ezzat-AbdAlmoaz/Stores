using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mkhz.Migrations
{
    /// <inheritdoc />
    public partial class BB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_productWithQuantities_invoices_InvoiceId",
                table: "productWithQuantities");

            migrationBuilder.DropIndex(
                name: "IX_productWithQuantities_InvoiceId",
                table: "productWithQuantities");

            migrationBuilder.AlterColumn<int>(
                name: "InvoiceId",
                table: "productWithQuantities",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "InvoiceId",
                table: "productWithQuantities",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_productWithQuantities_InvoiceId",
                table: "productWithQuantities",
                column: "InvoiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_productWithQuantities_invoices_InvoiceId",
                table: "productWithQuantities",
                column: "InvoiceId",
                principalTable: "invoices",
                principalColumn: "Id");
        }
    }
}
