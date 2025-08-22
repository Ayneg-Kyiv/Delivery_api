using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.ShippingDb
{
    /// <inheritdoc />
    public partial class RemoveShippingDestinationFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShippingDestinations_ShippingOrders_ShippingOrderId",
                table: "ShippingDestinations");

            migrationBuilder.DropIndex(
                name: "IX_ShippingDestinations_ShippingOrderId",
                table: "ShippingDestinations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ShippingDestinations_ShippingOrderId",
                table: "ShippingDestinations",
                column: "ShippingOrderId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ShippingDestinations_ShippingOrders_ShippingOrderId",
                table: "ShippingDestinations",
                column: "ShippingOrderId",
                principalTable: "ShippingOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
