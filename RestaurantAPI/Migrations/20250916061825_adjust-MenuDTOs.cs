using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantAPI.Migrations
{
    /// <inheritdoc />
    public partial class adjustMenuDTOs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Table_TableNumber",
                table: "Table");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Menu",
                newName: "DishName");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Menu",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DishName",
                table: "Menu",
                newName: "Name");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Menu",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Table_TableNumber",
                table: "Table",
                column: "TableNumber",
                unique: true);
        }
    }
}
