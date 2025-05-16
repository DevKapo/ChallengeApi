using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChallengeApi.Migrations
{
    /// <inheritdoc />
    public partial class CambiarValorPortada : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PeliculaNombre",
                table: "Portadas");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PeliculaNombre",
                table: "Portadas",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
