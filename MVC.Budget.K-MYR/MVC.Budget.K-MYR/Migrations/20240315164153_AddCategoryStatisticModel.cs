using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVC.Budget.K_MYR.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryStatisticModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoryStatistics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Budget = table.Column<decimal>(type: "decimal(19,4)", precision: 19, scale: 4, nullable: false),
                    Month = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalSpent = table.Column<decimal>(type: "decimal(19,4)", precision: 19, scale: 4, nullable: false),
                    Overspending = table.Column<decimal>(type: "decimal(19,4)", precision: 19, scale: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryStatistics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoryStatistics_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryStatistics_CategoryId",
                table: "CategoryStatistics",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryStatistics");
        }
    }
}
