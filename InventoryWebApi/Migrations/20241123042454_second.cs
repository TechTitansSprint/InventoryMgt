using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryWebApi.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Supplier_SupplierId",
                table: "Product");

            migrationBuilder.RenameColumn(
                name: "SupplierId",
                table: "Product",
                newName: "SupplierID");

            migrationBuilder.RenameIndex(
                name: "IX_Product_SupplierId",
                table: "Product",
                newName: "IX_Product_SupplierID");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Supplier_SupplierID",
                table: "Product",
                column: "SupplierID",
                principalTable: "Supplier",
                principalColumn: "SupplierId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Supplier_SupplierID",
                table: "Product");

            migrationBuilder.RenameColumn(
                name: "SupplierID",
                table: "Product",
                newName: "SupplierId");

            migrationBuilder.RenameIndex(
                name: "IX_Product_SupplierID",
                table: "Product",
                newName: "IX_Product_SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Supplier_SupplierId",
                table: "Product",
                column: "SupplierId",
                principalTable: "Supplier",
                principalColumn: "SupplierId");
        }
    }
}
