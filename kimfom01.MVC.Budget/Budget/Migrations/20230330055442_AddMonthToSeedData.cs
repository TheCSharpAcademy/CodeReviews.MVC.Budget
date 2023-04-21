using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Budget.Migrations
{
    /// <inheritdoc />
    public partial class AddMonthToSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Date", "Month" },
                values: new object[] { new DateTime(2023, 3, 30, 8, 54, 42, 354, DateTimeKind.Local).AddTicks(4854), 3 });

            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Date", "Month" },
                values: new object[] { new DateTime(2023, 3, 30, 8, 54, 42, 354, DateTimeKind.Local).AddTicks(4906), 3 });

            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Date", "Month" },
                values: new object[] { new DateTime(2023, 3, 30, 8, 54, 42, 354, DateTimeKind.Local).AddTicks(4920), 3 });

            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Date", "Month" },
                values: new object[] { new DateTime(2023, 3, 30, 8, 54, 42, 354, DateTimeKind.Local).AddTicks(4934), 3 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Date", "Month" },
                values: new object[] { new DateTime(2023, 3, 19, 23, 15, 43, 934, DateTimeKind.Local).AddTicks(7723), null });

            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Date", "Month" },
                values: new object[] { new DateTime(2023, 3, 19, 23, 15, 43, 934, DateTimeKind.Local).AddTicks(7734), null });

            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Date", "Month" },
                values: new object[] { new DateTime(2023, 3, 19, 23, 15, 43, 934, DateTimeKind.Local).AddTicks(7736), null });

            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Date", "Month" },
                values: new object[] { new DateTime(2023, 3, 19, 23, 15, 43, 934, DateTimeKind.Local).AddTicks(7738), null });
        }
    }
}
