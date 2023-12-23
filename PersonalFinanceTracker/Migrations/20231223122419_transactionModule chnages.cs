using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalFinanceTracker.Migrations
{
    /// <inheritdoc />
    public partial class transactionModulechnages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TransactionDescription",
                table: "Transaction",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionDescription",
                table: "Transaction");
        }
    }
}
