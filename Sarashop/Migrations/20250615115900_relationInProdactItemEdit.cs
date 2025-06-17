using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sarashop.Migrations
{
    /// <inheritdoc />
    public partial class relationInProdactItemEdit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OrdersItems",
                table: "OrdersItems");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "OrdersItems");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrdersItems",
                table: "OrdersItems",
                columns: new[] { "OrderId", "ProdactId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OrdersItems",
                table: "OrdersItems");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "OrdersItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrdersItems",
                table: "OrdersItems",
                columns: new[] { "OrderId", "ProductId" });
        }
    }
}
