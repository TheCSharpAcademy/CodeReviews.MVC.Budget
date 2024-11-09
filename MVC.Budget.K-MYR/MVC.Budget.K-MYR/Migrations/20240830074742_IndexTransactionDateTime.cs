using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVC.Budget.K_MYR.Migrations
{
    /// <inheritdoc />
    public partial class IndexTransactionDateTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Transactions_DateTime",
                table: "Transactions",
                column: "DateTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Transactions_DateTime",
                table: "Transactions");
        }
    }
}
