using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventCorp.Migrations
{
    /// <inheritdoc />
    public partial class CambioColumnaDuracionEvento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                 name: "Duracion",
                 table: "Eventos");

            migrationBuilder.AddColumn<int>(
                name: "Duracion",
                table: "Eventos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duracion",
                table: "Eventos");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Duracion",
                table: "Eventos",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0));
        }
    }
}
