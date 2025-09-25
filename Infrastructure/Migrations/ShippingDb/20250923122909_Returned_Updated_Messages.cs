using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.ShippingDb
{
    /// <inheritdoc />
    public partial class Returned_Updated_Messages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SenderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReceiverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeliveryOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeliveryOfferId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SeenAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_DeliveryOffers_DeliveryOfferId",
                        column: x => x.DeliveryOfferId,
                        principalTable: "DeliveryOffers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Messages_DeliveryOrders_DeliveryOrderId",
                        column: x => x.DeliveryOrderId,
                        principalTable: "DeliveryOrders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_DeliveryOfferId",
                table: "Messages",
                column: "DeliveryOfferId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_DeliveryOrderId",
                table: "Messages",
                column: "DeliveryOrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Messages");
        }
    }
}
