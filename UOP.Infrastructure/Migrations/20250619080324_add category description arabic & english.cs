using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UOP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addcategorydescriptionarabicenglish : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                schema: "Core",
                table: "Category");

            migrationBuilder.AddColumn<string>(
                name: "Description_Ar",
                schema: "Core",
                table: "Category",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description_En",
                schema: "Core",
                table: "Category",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description_Ar",
                schema: "Core",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "Description_En",
                schema: "Core",
                table: "Category");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "Core",
                table: "Category",
                type: "nvarchar(800)",
                maxLength: 800,
                nullable: true);
        }
    }
}
