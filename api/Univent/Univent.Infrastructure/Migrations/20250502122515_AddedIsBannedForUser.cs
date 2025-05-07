using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Univent.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedIsBannedForUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAccountBanned",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAccountBanned",
                table: "AspNetUsers");
        }
    }
}
