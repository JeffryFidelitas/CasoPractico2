using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreLibrary.Migrations
{
    /// <inheritdoc />
    public partial class ModificacionDeDatos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "IdCategoria",
                keyValue: 1,
                column: "FechaRegistro",
                value: new DateTime(2025, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "IdCategoria",
                keyValue: 2,
                column: "FechaRegistro",
                value: new DateTime(2025, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "NombreCompleto", "NombreUsuario" },
                values: new object[] { "UsuarioUNO Regular", "usuarioUNO" });

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "NombreCompleto", "NombreUsuario" },
                values: new object[] { "UsuarioDOS Regular", "usuarioDOS" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "IdCategoria",
                keyValue: 1,
                column: "FechaRegistro",
                value: new DateTime(2025, 4, 17, 19, 31, 26, 878, DateTimeKind.Local).AddTicks(6649));

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "IdCategoria",
                keyValue: 2,
                column: "FechaRegistro",
                value: new DateTime(2025, 4, 17, 19, 31, 26, 878, DateTimeKind.Local).AddTicks(6651));

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "NombreCompleto", "NombreUsuario" },
                values: new object[] { "Usuario1 Regular", "usuario1" });

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "NombreCompleto", "NombreUsuario" },
                values: new object[] { "Usuario2 Regular", "usuario2" });
        }
    }
}
