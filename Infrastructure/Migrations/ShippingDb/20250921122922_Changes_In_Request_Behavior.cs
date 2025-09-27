using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.ShippingDb
{
    /// <inheritdoc />
    public partial class Changes_In_Request_Behavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_DeliveryOffers_DeliveryRequestId",
                table: "DeliveryOffers",
                column: "DeliveryRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryOffers_DeliveryRequests_DeliveryRequestId",
                table: "DeliveryOffers",
                column: "DeliveryRequestId",
                principalTable: "DeliveryRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryOffers_DeliveryRequests_DeliveryRequestId",
                table: "DeliveryOffers");

            migrationBuilder.DropIndex(
                name: "IX_DeliveryOffers_DeliveryRequestId",
                table: "DeliveryOffers");
        }
    }
}
