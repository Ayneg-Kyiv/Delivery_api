using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.ShippingDb
{
    /// <inheritdoc />
    public partial class Changes_In_Delivery_Order_Adn_Offer_States_Of_Object : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CargoSlotType",
                table: "DeliveryRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "EstimatedPrice",
                table: "DeliveryRequests",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeclined",
                table: "DeliveryOffers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CargoSlotType",
                table: "DeliveryRequests");

            migrationBuilder.DropColumn(
                name: "EstimatedPrice",
                table: "DeliveryRequests");

            migrationBuilder.DropColumn(
                name: "IsDeclined",
                table: "DeliveryOffers");
        }
    }
}
