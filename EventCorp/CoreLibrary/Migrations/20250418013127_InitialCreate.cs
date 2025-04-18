using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CoreLibrary.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreUsuario = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NombreCompleto = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Contrasena = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Rol = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    IdCategoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioRegistroId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.IdCategoria);
                    table.ForeignKey(
                        name: "FK_Categorias_Usuarios_UsuarioRegistroId",
                        column: x => x.UsuarioRegistroId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Errores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: true),
                    Mensaje = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    StackTrace = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Origen = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Errores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Errores_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Eventos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CategoriaId = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Hora = table.Column<TimeSpan>(type: "time", nullable: false),
                    Duracion = table.Column<int>(type: "int", nullable: false),
                    Ubicacion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CupoMaximo = table.Column<int>(type: "int", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioRegistroId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eventos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Eventos_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "IdCategoria",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Eventos_Usuarios_UsuarioRegistroId",
                        column: x => x.UsuarioRegistroId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Contrasena", "Correo", "NombreCompleto", "NombreUsuario", "Rol", "Telefono" },
                values: new object[,]
                {
                    { 1, "Admin123", "admin@eventcorp.com", "Administrador General", "admin", 1, "+50688888888" },
                    { 2, "Organizador123", "organizador@eventcorp.com", "Organizador de Eventos", "organizador", 2, "+50677777777" },
                    { 3, "Usuario123", "usuario1@eventcorp.com", "Usuario1 Regular", "usuario1", 3, "+50666666666" },
                    { 4, "Usuario123", "usuario2@eventcorp.com", "Usuario2 Regular", "usuario2", 3, "+50666666664" }
                });

            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "IdCategoria", "Descripcion", "Estado", "FechaRegistro", "Nombre", "UsuarioRegistroId" },
                values: new object[,]
                {
                    { 1, "Eventos de tipo conferencia.", true, new DateTime(2025, 4, 17, 19, 31, 26, 878, DateTimeKind.Local).AddTicks(6649), "Conferencia", 1 },
                    { 2, "Eventos de tipo taller.", true, new DateTime(2025, 4, 17, 19, 31, 26, 878, DateTimeKind.Local).AddTicks(6651), "Taller", 1 }
                });

            migrationBuilder.InsertData(
                table: "Eventos",
                columns: new[] { "Id", "CategoriaId", "CupoMaximo", "Descripcion", "Duracion", "Fecha", "FechaRegistro", "Hora", "Titulo", "Ubicacion", "UsuarioRegistroId" },
                values: new object[,]
                {
                    { 1, 1, 100, "Evento que reúne a expertos en tecnología para discutir innovaciones recientes.", 120, new DateTime(2025, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 14, 0, 0, 0), "Conferencia sobre Innovación Tecnológica", "Centro de Convenciones, San José", 3 },
                    { 2, 2, 50, "Taller práctico sobre tecnologías modernas para el desarrollo web.", 180, new DateTime(2025, 6, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 9, 0, 0, 0), "Taller de Desarrollo Web", "Auditorio Universidad Fidélitas", 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_UsuarioRegistroId",
                table: "Categorias",
                column: "UsuarioRegistroId");

            migrationBuilder.CreateIndex(
                name: "IX_Errores_UsuarioId",
                table: "Errores",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_CategoriaId",
                table: "Eventos",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_UsuarioRegistroId",
                table: "Eventos",
                column: "UsuarioRegistroId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Errores");

            migrationBuilder.DropTable(
                name: "Eventos");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
