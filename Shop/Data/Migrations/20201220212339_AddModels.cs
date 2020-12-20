using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Shop.Data.Migrations
{
    public partial class AddModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BasketID",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Basket",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Basket", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "OrderStatus",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStatus", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Item",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Details = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    SalePrice = table.Column<decimal>(type: "decimal(18, 4)", nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    CategoryID = table.Column<int>(nullable: true),
                    BasketID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Item_Basket_BasketID",
                        column: x => x.BasketID,
                        principalTable: "Basket",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Item_Category_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Category",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    StatusID = table.Column<int>(nullable: true),
                    ApplicationUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Order_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Order_OrderStatus_StatusID",
                        column: x => x.StatusID,
                        principalTable: "OrderStatus",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BasketItem",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductID = table.Column<int>(nullable: true),
                    BasketID = table.Column<int>(nullable: true),
                    OrderID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasketItem", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BasketItem_Basket_BasketID",
                        column: x => x.BasketID,
                        principalTable: "Basket",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BasketItem_Order_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Order",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BasketItem_Item_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Item",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_BasketID",
                table: "AspNetUsers",
                column: "BasketID");

            migrationBuilder.CreateIndex(
                name: "IX_BasketItem_BasketID",
                table: "BasketItem",
                column: "BasketID");

            migrationBuilder.CreateIndex(
                name: "IX_BasketItem_OrderID",
                table: "BasketItem",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_BasketItem_ProductID",
                table: "BasketItem",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Item_BasketID",
                table: "Item",
                column: "BasketID");

            migrationBuilder.CreateIndex(
                name: "IX_Item_CategoryID",
                table: "Item",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_ApplicationUserId",
                table: "Order",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_StatusID",
                table: "Order",
                column: "StatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Basket_BasketID",
                table: "AspNetUsers",
                column: "BasketID",
                principalTable: "Basket",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Basket_BasketID",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "BasketItem");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Item");

            migrationBuilder.DropTable(
                name: "OrderStatus");

            migrationBuilder.DropTable(
                name: "Basket");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_BasketID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BasketID",
                table: "AspNetUsers");
        }
    }
}
