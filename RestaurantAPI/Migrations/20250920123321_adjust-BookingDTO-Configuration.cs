using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantAPI.Migrations
{
    /// <inheritdoc />
    public partial class adjustBookingDTOConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Customer_FK_CustomerId",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Table_FK_TableId",
                table: "Booking");

            migrationBuilder.DropTable(
                name: "ResturantConfiguration");

            migrationBuilder.DropIndex(
                name: "IX_Booking_FK_CustomerId",
                table: "Booking");

            migrationBuilder.DropIndex(
                name: "IX_Booking_FK_TableId",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Booking");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Booking",
                newName: "status");

            migrationBuilder.AlterColumn<int>(
                name: "status",
                table: "Booking",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateAt",
                table: "Booking",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "Booking",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TableId",
                table: "Booking",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Booking",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Booking_CustomerId",
                table: "Booking",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_TableId",
                table: "Booking",
                column: "TableId");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Customer_CustomerId",
                table: "Booking",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Table_TableId",
                table: "Booking",
                column: "TableId",
                principalTable: "Table",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Customer_CustomerId",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Table_TableId",
                table: "Booking");

            migrationBuilder.DropIndex(
                name: "IX_Booking_CustomerId",
                table: "Booking");

            migrationBuilder.DropIndex(
                name: "IX_Booking_TableId",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "CreateAt",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "TableId",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Booking");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "Booking",
                newName: "Status");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Booking",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "EndTime",
                table: "Booking",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.CreateTable(
                name: "ResturantConfiguration",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookingSlotInterval = table.Column<TimeSpan>(type: "time", nullable: false),
                    ClosingTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    DefaultBookingDuration = table.Column<TimeSpan>(type: "time", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OpeningTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResturantConfiguration", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Booking_FK_CustomerId",
                table: "Booking",
                column: "FK_CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_FK_TableId",
                table: "Booking",
                column: "FK_TableId");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Customer_FK_CustomerId",
                table: "Booking",
                column: "FK_CustomerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Table_FK_TableId",
                table: "Booking",
                column: "FK_TableId",
                principalTable: "Table",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
