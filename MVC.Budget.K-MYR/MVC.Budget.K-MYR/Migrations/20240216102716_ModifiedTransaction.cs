using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVC.Budget.K_MYR.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsHappy",
                table: "Transactions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsNecessary",
                table: "Transactions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHappy",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "IsNecessary",
                table: "Transactions");
        }
    }
}
