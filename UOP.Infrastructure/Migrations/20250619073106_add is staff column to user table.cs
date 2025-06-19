using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UOP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addisstaffcolumntousertable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsStaff",
                schema: "UserManagement",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsStaff",
                schema: "UserManagement",
                table: "User");
        }
    }
}
