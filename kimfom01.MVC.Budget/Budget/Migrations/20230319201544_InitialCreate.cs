using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Budget.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Wallets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Income = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Expenses = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    Balance = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Month = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Cost = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    WalletId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transactions_Wallets_WalletId",
                        column: x => x.WalletId,
                        principalTable: "Wallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Housing" },
                    { 2, "Transportation" },
                    { 3, "Food" },
                    { 4, "Utilities" },
                    { 5, "Insurance" },
                    { 6, "Medical & Healthcare" },
                    { 7, "Saving, Investing & Dept Payments" },
                    { 8, "Personal Spending" },
                    { 9, "Recreation & Entertainment" },
                    { 10, "Miscellaneous" }
                });

            migrationBuilder.InsertData(
                table: "Wallets",
                columns: new[] { "Id", "Balance", "Expenses", "Income", "Name" },
                values: new object[,]
                {
                    { 1, 4200m, 800m, 5000.00m, "Test Wallet" },
                    { 2, 4700.00m, 300m, 5000.00m, "Main Wallet" }
                });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "Id", "CategoryId", "Cost", "Date", "Description", "Month", "Name", "WalletId" },
                values: new object[,]
                {
                    { 1, 4, 500.00m, new DateTime(2023, 3, 19, 23, 15, 43, 934, DateTimeKind.Local).AddTicks(7723), "I bought a new laptop, external keyboard and mouse", null, "Computer Accessories", 1 },
                    { 2, 3, 150.00m, new DateTime(2023, 3, 19, 23, 15, 43, 934, DateTimeKind.Local).AddTicks(7734), "I bought a bunch of bananas, grapes and 7 oranges", null, "Weekly fruit stocking", 1 },
                    { 3, 2, 150.00m, new DateTime(2023, 3, 19, 23, 15, 43, 934, DateTimeKind.Local).AddTicks(7736), "Went to assist Dominion in her cake business", null, "Trip to Belgorod", 1 },
                    { 4, 5, 300.00m, new DateTime(2023, 3, 19, 23, 15, 43, 934, DateTimeKind.Local).AddTicks(7738), "Paid Annual Health Insurance", null, "Annual Health Insurance", 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CategoryId",
                table: "Transactions",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_WalletId",
                table: "Transactions",
                column: "WalletId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Wallets");
        }
    }
}
