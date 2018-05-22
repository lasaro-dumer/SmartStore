using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SmartStore.Data.Migrations
{
    public partial class v04 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StockMovementTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockMovementTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StockMoviments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<int>(nullable: false),
                    Balance = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    MovementTypeId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockMoviments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockMoviments_StockMovementTypes_MovementTypeId",
                        column: x => x.MovementTypeId,
                        principalTable: "StockMovementTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockMoviments_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockMoviments_MovementTypeId",
                table: "StockMoviments",
                column: "MovementTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMoviments_ProductId",
                table: "StockMoviments",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockMoviments");

            migrationBuilder.DropTable(
                name: "StockMovementTypes");
        }
    }
}
