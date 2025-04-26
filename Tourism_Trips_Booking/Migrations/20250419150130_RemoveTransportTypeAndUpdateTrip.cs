using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tourism_Trips_Booking.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTransportTypeAndUpdateTrip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Transport_Type_TranspirtTypeID",
                table: "Trips");

            migrationBuilder.DropTable(
                name: "Transport_Type");

            migrationBuilder.DropIndex(
                name: "IX_Trips_TranspirtTypeID",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "TranspirtTypeID",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "Depart",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "FromWhere",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "Return",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "ToWhere",
                table: "Booking");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "Trips",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Trips",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AddColumn<string>(
                name: "Destination",
                table: "Trips",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HotelName",
                table: "Trips",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TransportType",
                table: "Trips",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Destination",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "HotelName",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "TransportType",
                table: "Trips");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "StartDate",
                table: "Trips",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "EndDate",
                table: "Trips",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "TranspirtTypeID",
                table: "Trips",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateOnly>(
                name: "Depart",
                table: "Booking",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "FromWhere",
                table: "Booking",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateOnly>(
                name: "Return",
                table: "Booking",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "ToWhere",
                table: "Booking",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Transport_Type",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transport_Type", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Trips_TranspirtTypeID",
                table: "Trips",
                column: "TranspirtTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Transport_Type_TranspirtTypeID",
                table: "Trips",
                column: "TranspirtTypeID",
                principalTable: "Transport_Type",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
