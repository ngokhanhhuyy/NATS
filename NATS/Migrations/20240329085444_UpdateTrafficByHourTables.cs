using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NATS.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTrafficByHourTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_traffic_by_hour_recorded_at",
                table: "traffic_by_hour");

            migrationBuilder.RenameIndex(
                name: "unique_users_username",
                table: "users",
                newName: "unique__users__username");

            migrationBuilder.RenameIndex(
                name: "unique_roles_name",
                table: "roles",
                newName: "unique__roles__name");

            migrationBuilder.RenameIndex(
                name: "unique_roles_display_name",
                table: "roles",
                newName: "unique__roles__display_name");

            migrationBuilder.RenameIndex(
                name: "unique_products_name",
                table: "products",
                newName: "unique__products__name");

            migrationBuilder.RenameIndex(
                name: "unique_post_normalized_name",
                table: "posts",
                newName: "unique__post__normalized_name");

            migrationBuilder.RenameIndex(
                name: "unique_homepage_slider_items_index",
                table: "homepage_slider_items",
                newName: "unique__homepage_slider_items__index");

            migrationBuilder.RenameIndex(
                name: "unique_courses_name",
                table: "courses",
                newName: "unique__courses__name");

            migrationBuilder.AddColumn<int>(
                name: "access_count",
                table: "traffic_by_hour_ip_address",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ip_address",
                table: "traffic_by_hour_ip_address",
                type: "varchar(15)",
                maxLength: 15,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "guess_count",
                table: "traffic_by_hour",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "unique__traffic_by_hour__recoreded_at",
                table: "traffic_by_hour",
                column: "recorded_at",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "unique__traffic_by_hour__recoreded_at",
                table: "traffic_by_hour");

            migrationBuilder.DropColumn(
                name: "access_count",
                table: "traffic_by_hour_ip_address");

            migrationBuilder.DropColumn(
                name: "ip_address",
                table: "traffic_by_hour_ip_address");

            migrationBuilder.DropColumn(
                name: "guess_count",
                table: "traffic_by_hour");

            migrationBuilder.RenameIndex(
                name: "unique__users__username",
                table: "users",
                newName: "unique_users_username");

            migrationBuilder.RenameIndex(
                name: "unique__roles__name",
                table: "roles",
                newName: "unique_roles_name");

            migrationBuilder.RenameIndex(
                name: "unique__roles__display_name",
                table: "roles",
                newName: "unique_roles_display_name");

            migrationBuilder.RenameIndex(
                name: "unique__products__name",
                table: "products",
                newName: "unique_products_name");

            migrationBuilder.RenameIndex(
                name: "unique__post__normalized_name",
                table: "posts",
                newName: "unique_post_normalized_name");

            migrationBuilder.RenameIndex(
                name: "unique__homepage_slider_items__index",
                table: "homepage_slider_items",
                newName: "unique_homepage_slider_items_index");

            migrationBuilder.RenameIndex(
                name: "unique__courses__name",
                table: "courses",
                newName: "unique_courses_name");

            migrationBuilder.CreateIndex(
                name: "IX_traffic_by_hour_recorded_at",
                table: "traffic_by_hour",
                column: "recorded_at");
        }
    }
}
