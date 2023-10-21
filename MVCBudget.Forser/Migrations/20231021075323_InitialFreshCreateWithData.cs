using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MVCBudget.Forser.Migrations
{
    /// <inheritdoc />
    public partial class InitialFreshCreateWithData : Migration
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
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionsId = table.Column<int>(type: "int", nullable: true)
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
                    Amount = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    TransactionsId = table.Column<int>(type: "int", nullable: true)
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
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransferredAmount = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    UserWalletId = table.Column<int>(type: "int", nullable: true)
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
                        name: "FK_Transactions_Wallets_UserWalletId",
                        column: x => x.UserWalletId,
                        principalTable: "Wallets",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name", "TransactionsId" },
                values: new object[,]
                {
                    { 1, "Groceries", null },
                    { 2, "Restaurants", null },
                    { 3, "Gasoline", null },
                    { 4, "Utilities", null },
                    { 5, "Rent/Mortgage", null },
                    { 6, "Entertainment", null },
                    { 7, "Healthcare", null },
                    { 8, "Transportation", null },
                    { 9, "Clothing", null },
                    { 10, "Electronics", null },
                    { 11, "Home Improvement", null },
                    { 12, "Gifts", null },
                    { 13, "Travel", null },
                    { 14, "Insurance", null },
                    { 15, "Education", null },
                    { 16, "Savings", null },
                    { 17, "Investments", null },
                    { 18, "Charity", null },
                    { 19, "Pets", null },
                    { 20, "Miscellaneous", null }
                });

            migrationBuilder.InsertData(
                table: "Wallets",
                columns: new[] { "Id", "Amount", "Name", "TransactionsId" },
                values: new object[] { 1, 10000m, "Marcus Wallet", null });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "Id", "CategoryId", "Description", "Name", "TransactionDate", "TransferredAmount", "UserWalletId" },
                values: new object[,]
                {
                    { 1, 1, "Weekly grocery expenses", "Grocery Shopping", new DateTime(2022, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), -50.00m, 1 },
                    { 2, 2, "Dining out with friends", "Dinner at Pizzeria", new DateTime(2022, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), -30.00m, 1 },
                    { 3, 3, "Car fuel refill", "Gas Refill", new DateTime(2022, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), -40.00m, 1 },
                    { 4, 4, "Monthly electricity bill", "Electric Bill", new DateTime(2022, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), -70.00m, 1 },
                    { 5, 5, "Monthly rent for apartment", "Rent Payment", new DateTime(2023, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), -1200.00m, 1 },
                    { 6, 6, "Entertainment expenses", "Movie Tickets", new DateTime(2022, 7, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), -25.00m, 1 },
                    { 7, 7, "Medical check-up copay", "Doctor's Visit", new DateTime(2021, 12, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), -40.00m, 1 },
                    { 8, 8, "Public transportation cost", "Bus Fare", new DateTime(2023, 2, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), -10.00m, 1 },
                    { 9, 9, "Footwear purchase", "New Shoes", new DateTime(2022, 6, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), -60.00m, 1 },
                    { 10, 10, "New laptop for work", "Laptop Purchase", new DateTime(2021, 11, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), -800.00m, 1 },
                    { 11, 11, "Fixing plumbing issues", "Home Repairs", new DateTime(2022, 2, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), -150.00m, 1 },
                    { 12, 12, "Gift for a friend's birthday", "Birthday Gift", new DateTime(2023, 4, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), -20.00m, 1 },
                    { 13, 13, "Short trip with family", "Weekend Getaway", new DateTime(2023, 7, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), -300.00m, 1 },
                    { 14, 14, "Monthly insurance premium", "Health Insurance", new DateTime(2022, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), -80.00m, 1 },
                    { 15, 15, "Purchase of college textbooks", "Textbooks", new DateTime(2021, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), -150.00m, 1 },
                    { 16, 16, "Adding to savings account", "Savings Deposit", new DateTime(2023, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 200.00m, 1 },
                    { 17, 17, "Investing in mutual funds", "Investment Fund", new DateTime(2023, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 500.00m, 1 },
                    { 18, 18, "Contribution to a local charity", "Charity Donation", new DateTime(2021, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), -30.00m, 1 },
                    { 19, 19, "Buying food and toys for pets", "Pet Supplies", new DateTime(2022, 1, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), -45.00m, 1 },
                    { 20, 20, "Random miscellaneous expense", "Miscellaneous", new DateTime(2023, 8, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), -15.00m, 1 },
                    { 21, 2, "Morning coffee and pastries", "Coffee Shop", new DateTime(2023, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), -8.50m, 1 },
                    { 22, 1, "Weekly grocery expenses", "Grocery Shopping", new DateTime(2022, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), -60.00m, 1 },
                    { 23, 4, "Monthly internet subscription", "Internet Bill", new DateTime(2023, 6, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), -55.00m, 1 },
                    { 24, 6, "Entertainment expenses", "Movie Tickets", new DateTime(2022, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), -20.00m, 1 },
                    { 25, 7, "Dental check-up copay", "Dentist Appointment", new DateTime(2021, 10, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), -60.00m, 1 },
                    { 26, 8, "Public transportation cost", "Subway Fare", new DateTime(2022, 7, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), -5.50m, 1 },
                    { 27, 9, "Purchase of winter jacket", "New Jacket", new DateTime(2023, 2, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), -70.00m, 1 },
                    { 28, 10, "Upgrading to the latest model", "Smartphone Upgrade", new DateTime(2022, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), -700.00m, 1 },
                    { 29, 11, "Home improvement project", "Bathroom Renovation", new DateTime(2021, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), -200.00m, 1 },
                    { 30, 12, "Gift for spouse's anniversary", "Anniversary Gift", new DateTime(2023, 9, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), -50.00m, 1 },
                    { 31, 13, "Summer vacation with family", "Beach Vacation", new DateTime(2022, 8, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), -800.00m, 1 },
                    { 32, 14, "Auto insurance premium", "Car Insurance", new DateTime(2021, 6, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), -90.00m, 1 },
                    { 33, 15, "Back-to-school shopping", "School Supplies", new DateTime(2023, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), -40.00m, 1 },
                    { 34, 17, "Adding to investment portfolio", "Investment Portfolio", new DateTime(2023, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 300.00m, 1 },
                    { 35, 20, "Purchase of home decor items", "Home Decor", new DateTime(2022, 12, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), -35.00m, 1 },
                    { 36, 4, "Monthly phone plan bill", "Phone Bill", new DateTime(2022, 9, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), -40.00m, 1 },
                    { 37, 8, "Ride to the airport", "Taxi Ride", new DateTime(2021, 11, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), -25.00m, 1 },
                    { 38, 15, "Monthly gym membership", "Fitness Membership", new DateTime(2023, 7, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), -60.00m, 1 },
                    { 39, 6, "New books and magazines", "Bookstore Purchase", new DateTime(2022, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), -45.00m, 1 },
                    { 40, 11, "Routine car maintenance", "Car Maintenance", new DateTime(2022, 6, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), -80.00m, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CategoryId",
                table: "Transactions",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_UserWalletId",
                table: "Transactions",
                column: "UserWalletId");
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
