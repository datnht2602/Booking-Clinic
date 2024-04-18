using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clinic.Identity.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCreatedTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CreatedTime",
                table: "AspNetUsers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "AspNetUsers");
        }
    }
}
