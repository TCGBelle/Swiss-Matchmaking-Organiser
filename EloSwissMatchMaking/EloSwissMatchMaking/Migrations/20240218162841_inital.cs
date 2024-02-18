using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EloSwissMatchMaking.Migrations
{
    /// <inheritdoc />
    public partial class inital : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "_name",
                table: "Players",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "_elo",
                table: "Players",
                newName: "ELO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Players",
                newName: "_name");

            migrationBuilder.RenameColumn(
                name: "ELO",
                table: "Players",
                newName: "_elo");
        }
    }
}
