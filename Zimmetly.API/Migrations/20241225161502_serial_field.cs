using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zimmetly.API.Migrations
{
    /// <inheritdoc />
    public partial class serial_field : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Serial",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Serial",
                table: "Products");
        }
    }
}
