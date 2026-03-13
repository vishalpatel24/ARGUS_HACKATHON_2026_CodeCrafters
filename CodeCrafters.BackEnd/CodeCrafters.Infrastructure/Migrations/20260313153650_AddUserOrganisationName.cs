using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeCrafters.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserOrganisationName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrganisationName",
                table: "Users",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrganisationName",
                table: "Users");
        }
    }
}
