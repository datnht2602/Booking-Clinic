using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clinic.Identity.Migrations
{
    /// <inheritdoc />
    public partial class _20240406_UpdateFeedBack : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeedBack_AspNetUsers_DoctorId",
                table: "FeedBack");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FeedBack",
                table: "FeedBack");

            migrationBuilder.RenameTable(
                name: "FeedBack",
                newName: "FeedBacks");

            migrationBuilder.RenameIndex(
                name: "IX_FeedBack_DoctorId",
                table: "FeedBacks",
                newName: "IX_FeedBacks_DoctorId");

            migrationBuilder.AddColumn<string>(
                name: "BookingId",
                table: "FeedBacks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FeedBacks",
                table: "FeedBacks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FeedBacks_AspNetUsers_DoctorId",
                table: "FeedBacks",
                column: "DoctorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeedBacks_AspNetUsers_DoctorId",
                table: "FeedBacks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FeedBacks",
                table: "FeedBacks");

            migrationBuilder.DropColumn(
                name: "BookingId",
                table: "FeedBacks");

            migrationBuilder.RenameTable(
                name: "FeedBacks",
                newName: "FeedBack");

            migrationBuilder.RenameIndex(
                name: "IX_FeedBacks_DoctorId",
                table: "FeedBack",
                newName: "IX_FeedBack_DoctorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FeedBack",
                table: "FeedBack",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FeedBack_AspNetUsers_DoctorId",
                table: "FeedBack",
                column: "DoctorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
