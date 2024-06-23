using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SzczurApp.Migrations
{
    /// <inheritdoc />
    public partial class AddStreamRelatedFieldsApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "AspNetUsers",
                type: "text",
                nullable: true
            );

            migrationBuilder.AddColumn<string>(
                name: "StreamCategory",
                table: "AspNetUsers",
                type: "text",
                nullable: true
            );

            migrationBuilder.AddColumn<string>(
                name: "StreamTitle",
                table: "AspNetUsers",
                type: "text",
                nullable: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "Bio", table: "AspNetUsers");

            migrationBuilder.DropColumn(name: "StreamCategory", table: "AspNetUsers");

            migrationBuilder.DropColumn(name: "StreamTitle", table: "AspNetUsers");
        }
    }
}
