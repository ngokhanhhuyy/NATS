using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NATS.Migrations
{
    /// <inheritdoc />
    public partial class AddGeneralSettingsUnderMaintainanceColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "under_maintainance",
                table: "general_settings",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "under_maintainance",
                table: "general_settings");
        }
    }
}
