using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.ShippingDb
{
    /// <inheritdoc />
    public partial class RemoveCrossDatabaseForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_ApplicationUser_ReceiverId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_ApplicationUser_SenderId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_ApplicationUser_UserId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_ShippingOffers_ApplicationUser_CourierId",
                table: "ShippingOffers");

            migrationBuilder.DropForeignKey(
                name: "FK_ShippingOrders_ApplicationUser_CustomerId",
                table: "ShippingOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_ApplicationUser_OwnerId",
                table: "Vehicles");

            migrationBuilder.DropTable(
                name: "SessionData");

            migrationBuilder.DropTable(
                name: "ApplicationUser");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_OwnerId",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_ShippingOrders_CustomerId",
                table: "ShippingOrders");

            migrationBuilder.DropIndex(
                name: "IX_ShippingOffers_CourierId",
                table: "ShippingOffers");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Messages_ReceiverId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_SenderId",
                table: "Messages");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationUser",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AboutMe = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    Rating = table.Column<float>(type: "real", nullable: false),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SessionData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeviceIdentifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstActive = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(45)", nullable: true),
                    LastActive = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserAgent = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SessionData_ApplicationUser_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_OwnerId",
                table: "Vehicles",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ShippingOrders_CustomerId",
                table: "ShippingOrders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ShippingOffers_CourierId",
                table: "ShippingOffers",
                column: "CourierId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ReceiverId",
                table: "Messages",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderId",
                table: "Messages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_Email",
                table: "ApplicationUser",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_SessionData_RefreshToken",
                table: "SessionData",
                column: "RefreshToken",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SessionData_UserId",
                table: "SessionData",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_ApplicationUser_ReceiverId",
                table: "Messages",
                column: "ReceiverId",
                principalTable: "ApplicationUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_ApplicationUser_SenderId",
                table: "Messages",
                column: "SenderId",
                principalTable: "ApplicationUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_ApplicationUser_UserId",
                table: "Reviews",
                column: "UserId",
                principalTable: "ApplicationUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShippingOffers_ApplicationUser_CourierId",
                table: "ShippingOffers",
                column: "CourierId",
                principalTable: "ApplicationUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShippingOrders_ApplicationUser_CustomerId",
                table: "ShippingOrders",
                column: "CustomerId",
                principalTable: "ApplicationUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_ApplicationUser_OwnerId",
                table: "Vehicles",
                column: "OwnerId",
                principalTable: "ApplicationUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
