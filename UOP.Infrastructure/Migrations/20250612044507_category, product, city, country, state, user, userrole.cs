using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UOP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class categoryproductcitycountrystateuseruserrole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Core");

            migrationBuilder.EnsureSchema(
                name: "UserManagement");

            migrationBuilder.CreateTable(
                name: "Category",
                schema: "Core",
                columns: table => new
                {
                    CategoryID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Name_Ar = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name_En = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Order = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(800)", maxLength: 800, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsParent = table.Column<bool>(type: "bit", nullable: false),
                    IconUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Core.Category", x => x.CategoryID);
                    table.ForeignKey(
                        name: "FK_Category_Self",
                        column: x => x.ParentId,
                        principalSchema: "Core",
                        principalTable: "Category",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Country",
                schema: "Core",
                columns: table => new
                {
                    CountryID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name_En = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name_Ar = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Order = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FlagImage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FlagThumbnail = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Core.Country", x => x.CountryID);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                schema: "UserManagement",
                columns: table => new
                {
                    RoleID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserManagement.Role", x => x.RoleID);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "UserManagement",
                columns: table => new
                {
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    PictureUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CoverPhotoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UserPictureUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserManagement.User", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                schema: "Core",
                columns: table => new
                {
                    ProductID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Name_Ar = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name_En = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Order = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(800)", maxLength: 800, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Core.Product", x => x.ProductID);
                    table.ForeignKey(
                        name: "FK_Product_Category",
                        column: x => x.CategoryId,
                        principalSchema: "Core",
                        principalTable: "Category",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "State",
                schema: "Core",
                columns: table => new
                {
                    StateID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name_En = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name_Ar = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Order = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MapUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Core.State", x => x.StateID);
                    table.ForeignKey(
                        name: "FK_State_Country",
                        column: x => x.CountryId,
                        principalSchema: "Core",
                        principalTable: "Country",
                        principalColumn: "CountryID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PhoneNumber",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PhoneNumberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhoneNumber", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhoneNumber_User",
                        column: x => x.UserId,
                        principalSchema: "UserManagement",
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                schema: "Core",
                columns: table => new
                {
                    UserRoleID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserManagement.UserRole", x => x.UserRoleID);
                    table.ForeignKey(
                        name: "FK_UserRole_Role",
                        column: x => x.RoleId,
                        principalSchema: "UserManagement",
                        principalTable: "Role",
                        principalColumn: "RoleID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRole_User",
                        column: x => x.UserId,
                        principalSchema: "UserManagement",
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "City",
                schema: "Core",
                columns: table => new
                {
                    CityID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name_En = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name_Ar = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Order = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Core.City", x => x.CityID);
                    table.ForeignKey(
                        name: "FK_City_State",
                        column: x => x.StateId,
                        principalSchema: "Core",
                        principalTable: "State",
                        principalColumn: "StateID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Category_Order",
                schema: "Core",
                table: "Category",
                column: "Order");

            migrationBuilder.CreateIndex(
                name: "IX_Category_ParentId",
                schema: "Core",
                table: "Category",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_City_Order",
                schema: "Core",
                table: "City",
                column: "Order");

            migrationBuilder.CreateIndex(
                name: "IX_City_StateId",
                schema: "Core",
                table: "City",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_Country_Order",
                schema: "Core",
                table: "Country",
                column: "Order");

            migrationBuilder.CreateIndex(
                name: "IX_PhoneNumber_UserId",
                table: "PhoneNumber",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_CategoryId",
                schema: "Core",
                table: "Product",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Order",
                schema: "Core",
                table: "Product",
                column: "Order");

            migrationBuilder.CreateIndex(
                name: "IX_State_CountryId",
                schema: "Core",
                table: "State",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_State_Order",
                schema: "Core",
                table: "State",
                column: "Order");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                schema: "Core",
                table: "UserRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_UserId",
                schema: "Core",
                table: "UserRole",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "City",
                schema: "Core");

            migrationBuilder.DropTable(
                name: "PhoneNumber");

            migrationBuilder.DropTable(
                name: "Product",
                schema: "Core");

            migrationBuilder.DropTable(
                name: "UserRole",
                schema: "Core");

            migrationBuilder.DropTable(
                name: "State",
                schema: "Core");

            migrationBuilder.DropTable(
                name: "Category",
                schema: "Core");

            migrationBuilder.DropTable(
                name: "Role",
                schema: "UserManagement");

            migrationBuilder.DropTable(
                name: "User",
                schema: "UserManagement");

            migrationBuilder.DropTable(
                name: "Country",
                schema: "Core");
        }
    }
}
