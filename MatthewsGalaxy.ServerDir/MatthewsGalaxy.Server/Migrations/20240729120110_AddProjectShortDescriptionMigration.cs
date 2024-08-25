using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MatthewsGalaxy.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectShortDescriptionMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "localImageLocation",
                table: "Projects",
                newName: "LocalImageLocation");

            migrationBuilder.AddColumn<string>(
                name: "ShortDescription",
                table: "Projects",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShortDescription",
                table: "Projects");

            migrationBuilder.RenameColumn(
                name: "LocalImageLocation",
                table: "Projects",
                newName: "localImageLocation");
        }
    }
}
