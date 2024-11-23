using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NATS.Migrations
{
    /// <inheritdoc />
    public partial class AddTrafficUserAgentColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "last_user_agent",
                table: "traffic_by_hour_ip_address",
                type: "varchar(10000)",
                maxLength: 10000,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "last_user_agent",
                table: "traffic_by_hour_ip_address");
        }
    }
}
