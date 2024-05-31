using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVC.Budget.frockett.Migrations
{
    /// <inheritdoc />
    public partial class ChangeIdAndRemoveSetterFromCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Categories",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Categories",
                newName: "CategoryId");
        }
    }
}
