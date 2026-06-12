using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommanCenter.API.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditTrailFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CampoModificado",
                table: "AuditoriaLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Razon",
                table: "AuditoriaLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UsuarioRol",
                table: "AuditoriaLogs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CampoModificado",
                table: "AuditoriaLogs");

            migrationBuilder.DropColumn(
                name: "Razon",
                table: "AuditoriaLogs");

            migrationBuilder.DropColumn(
                name: "UsuarioRol",
                table: "AuditoriaLogs");
        }
    }
}
