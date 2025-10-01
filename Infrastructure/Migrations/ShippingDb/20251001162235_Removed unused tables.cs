using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.ShippingDb
{
    /// <inheritdoc />
    public partial class Removedunusedtables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_ShippingOrders_ShippingOrderId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_ShippingDestinations_ShippingOrders_ShippingOrderId",
                table: "ShippingDestinations");

            migrationBuilder.DropForeignKey(
                name: "FK_ShippingObjects_ShippingOrders_ShippingOrderId",
                table: "ShippingObjects");

            migrationBuilder.DropForeignKey(
                name: "FK_ShippingOffers_ShippingOrders_ShippingOrderId",
                table: "ShippingOffers");

            migrationBuilder.DropTable(
                name: "ShippingStartingPoints");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShippingOrders",
                table: "ShippingOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShippingOffers",
                table: "ShippingOffers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShippingObjects",
                table: "ShippingObjects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShippingDestinations",
                table: "ShippingDestinations");

            migrationBuilder.RenameTable(
                name: "ShippingOrders",
                newName: "ShippingOrder");

            migrationBuilder.RenameTable(
                name: "ShippingOffers",
                newName: "ShippingOffer");

            migrationBuilder.RenameTable(
                name: "ShippingObjects",
                newName: "ShippingObject");

            migrationBuilder.RenameTable(
                name: "ShippingDestinations",
                newName: "ShippingDestination");

            migrationBuilder.RenameIndex(
                name: "IX_ShippingOffers_ShippingOrderId",
                table: "ShippingOffer",
                newName: "IX_ShippingOffer_ShippingOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_ShippingObjects_ShippingOrderId",
                table: "ShippingObject",
                newName: "IX_ShippingObject_ShippingOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_ShippingDestinations_ShippingOrderId",
                table: "ShippingDestination",
                newName: "IX_ShippingDestination_ShippingOrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShippingOrder",
                table: "ShippingOrder",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShippingOffer",
                table: "ShippingOffer",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShippingObject",
                table: "ShippingObject",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShippingDestination",
                table: "ShippingDestination",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_ShippingOrder_ShippingOrderId",
                table: "Reviews",
                column: "ShippingOrderId",
                principalTable: "ShippingOrder",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShippingDestination_ShippingOrder_ShippingOrderId",
                table: "ShippingDestination",
                column: "ShippingOrderId",
                principalTable: "ShippingOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShippingObject_ShippingOrder_ShippingOrderId",
                table: "ShippingObject",
                column: "ShippingOrderId",
                principalTable: "ShippingOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShippingOffer_ShippingOrder_ShippingOrderId",
                table: "ShippingOffer",
                column: "ShippingOrderId",
                principalTable: "ShippingOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_ShippingOrder_ShippingOrderId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_ShippingDestination_ShippingOrder_ShippingOrderId",
                table: "ShippingDestination");

            migrationBuilder.DropForeignKey(
                name: "FK_ShippingObject_ShippingOrder_ShippingOrderId",
                table: "ShippingObject");

            migrationBuilder.DropForeignKey(
                name: "FK_ShippingOffer_ShippingOrder_ShippingOrderId",
                table: "ShippingOffer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShippingOrder",
                table: "ShippingOrder");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShippingOffer",
                table: "ShippingOffer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShippingObject",
                table: "ShippingObject");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShippingDestination",
                table: "ShippingDestination");

            migrationBuilder.RenameTable(
                name: "ShippingOrder",
                newName: "ShippingOrders");

            migrationBuilder.RenameTable(
                name: "ShippingOffer",
                newName: "ShippingOffers");

            migrationBuilder.RenameTable(
                name: "ShippingObject",
                newName: "ShippingObjects");

            migrationBuilder.RenameTable(
                name: "ShippingDestination",
                newName: "ShippingDestinations");

            migrationBuilder.RenameIndex(
                name: "IX_ShippingOffer_ShippingOrderId",
                table: "ShippingOffers",
                newName: "IX_ShippingOffers_ShippingOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_ShippingObject_ShippingOrderId",
                table: "ShippingObjects",
                newName: "IX_ShippingObjects_ShippingOrderId");

            migrationBuilder.RenameIndex(
                name: "IX_ShippingDestination_ShippingOrderId",
                table: "ShippingDestinations",
                newName: "IX_ShippingDestinations_ShippingOrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShippingOrders",
                table: "ShippingOrders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShippingOffers",
                table: "ShippingOffers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShippingObjects",
                table: "ShippingObjects",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShippingDestinations",
                table: "ShippingDestinations",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ShippingStartingPoints",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShippingOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AdditionalInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    District = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    HouseNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Street = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingStartingPoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShippingStartingPoints_ShippingOrders_ShippingOrderId",
                        column: x => x.ShippingOrderId,
                        principalTable: "ShippingOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShippingStartingPoints_ShippingOrderId",
                table: "ShippingStartingPoints",
                column: "ShippingOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_ShippingOrders_ShippingOrderId",
                table: "Reviews",
                column: "ShippingOrderId",
                principalTable: "ShippingOrders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShippingDestinations_ShippingOrders_ShippingOrderId",
                table: "ShippingDestinations",
                column: "ShippingOrderId",
                principalTable: "ShippingOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShippingObjects_ShippingOrders_ShippingOrderId",
                table: "ShippingObjects",
                column: "ShippingOrderId",
                principalTable: "ShippingOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShippingOffers_ShippingOrders_ShippingOrderId",
                table: "ShippingOffers",
                column: "ShippingOrderId",
                principalTable: "ShippingOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
