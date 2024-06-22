using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SzczurApp.Migrations
{
    /// <inheritdoc />
    public partial class AddStreamKeyApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StreamKey",
                table: "AspNetUsers",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StreamKey",
                table: "AspNetUsers");
        }
    }
}
