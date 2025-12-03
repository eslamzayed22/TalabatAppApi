using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersistenceLayer.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixProductTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Products",
                type: "varchar(600)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(350)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Products",
                type: "varchar(350)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(600)");
        }
    }
}
