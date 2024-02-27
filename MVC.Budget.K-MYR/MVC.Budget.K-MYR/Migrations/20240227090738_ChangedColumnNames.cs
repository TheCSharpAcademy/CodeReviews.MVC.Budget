using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVC.Budget.K_MYR.Migrations
{
    /// <inheritdoc />
    public partial class ChangedColumnNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PreviousIsNecessaryValue",
                table: "Transactions",
                newName: "EvaluatedIsNecessary");

            migrationBuilder.RenameColumn(
                name: "PreviousIsHappyValue",
                table: "Transactions",
                newName: "EvaluatedIsHappy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EvaluatedIsNecessary",
                table: "Transactions",
                newName: "PreviousIsNecessaryValue");

            migrationBuilder.RenameColumn(
                name: "EvaluatedIsHappy",
                table: "Transactions",
                newName: "PreviousIsHappyValue");
        }
    }
}
