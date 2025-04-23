using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tourism_Trips_Booking.Migrations
{
    /// <inheritdoc />
    public partial class lastPaymentEdit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Manually_or_upon_receipt",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "PayPal",
                table: "Payment");

            migrationBuilder.AlterColumn<string>(
                name: "ImagePath",
                table: "Trips",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "Payment",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<string>(
                name: "CashOrCard",
                table: "Payment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CashOrCard",
                table: "Payment");

            migrationBuilder.AlterColumn<string>(
                name: "ImagePath",
                table: "Trips",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Payment",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<bool>(
                name: "Manually_or_upon_receipt",
                table: "Payment",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PayPal",
                table: "Payment",
                type: "bit",
                nullable: true);
        }
    }
}
