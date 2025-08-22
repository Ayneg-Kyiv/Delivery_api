using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.ShippingDb
{
    /// <inheritdoc />
    public partial class RemoveAllForeignKeyConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ShippingStartingPoints_ShippingOrderId",
                table: "ShippingStartingPoints");

            migrationBuilder.CreateIndex(
                name: "IX_ShippingStartingPoints_ShippingOrderId",
                table: "ShippingStartingPoints",
                column: "ShippingOrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ShippingStartingPoints_ShippingOrderId",
                table: "ShippingStartingPoints");

            migrationBuilder.CreateIndex(
                name: "IX_ShippingStartingPoints_ShippingOrderId",
                table: "ShippingStartingPoints",
                column: "ShippingOrderId",
                unique: true);
        }
    }
}
