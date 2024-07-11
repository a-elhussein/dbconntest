using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestIdentity.Migrations
{
    /// <inheritdoc />
    public partial class rolesupdatemanageractiveuseraddedadmintoroles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b4c0037d - 740b - 460d - ae4e - ada97e8d4219");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "UserCompanies",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsManager",
                table: "UserCompanies",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "26ac47fd-3a69-4894-9b95-1d7cf6550327", "26ac47fd-3a69-4894-9b95-1d7cf6550327", "Admin", "ADMIN" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "26ac47fd-3a69-4894-9b95-1d7cf6550327");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "UserCompanies");

            migrationBuilder.DropColumn(
                name: "IsManager",
                table: "UserCompanies");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b4c0037d - 740b - 460d - ae4e - ada97e8d4219", "b4c0037d - 740b - 460d - ae4e - ada97e8d4219", "Manager", "MANAGER" });
        }
    }
}
