using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PreAceleracionOctubre.Migrations
{
    public partial class First : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Disney");

            migrationBuilder.CreateTable(
                name: "Generos",
                schema: "Disney",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Imagen = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Generos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Personajes",
                schema: "Disney",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Imagen = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Edad = table.Column<int>(type: "int", nullable: false),
                    Peso = table.Column<int>(type: "int", nullable: false),
                    Historia = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personajes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Peliculas",
                schema: "Disney",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Imagen = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Calificacion = table.Column<int>(type: "int", nullable: false),
                    generoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Peliculas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Peliculas_Generos_generoId",
                        column: x => x.generoId,
                        principalSchema: "Disney",
                        principalTable: "Generos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PeliculaPersonaje",
                schema: "Disney",
                columns: table => new
                {
                    PeliculasId = table.Column<int>(type: "int", nullable: false),
                    PersonajesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeliculaPersonaje", x => new { x.PeliculasId, x.PersonajesId });
                    table.ForeignKey(
                        name: "FK_PeliculaPersonaje_Peliculas_PeliculasId",
                        column: x => x.PeliculasId,
                        principalSchema: "Disney",
                        principalTable: "Peliculas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PeliculaPersonaje_Personajes_PersonajesId",
                        column: x => x.PersonajesId,
                        principalSchema: "Disney",
                        principalTable: "Personajes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "Disney",
                table: "Personajes",
                columns: new[] { "Id", "Edad", "Historia", "Imagen", "Nombre", "Peso" },
                values: new object[] { 1, 80, "el raton", "img", "mickey", 3 });

            migrationBuilder.CreateIndex(
                name: "IX_PeliculaPersonaje_PersonajesId",
                schema: "Disney",
                table: "PeliculaPersonaje",
                column: "PersonajesId");

            migrationBuilder.CreateIndex(
                name: "IX_Peliculas_generoId",
                schema: "Disney",
                table: "Peliculas",
                column: "generoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PeliculaPersonaje",
                schema: "Disney");

            migrationBuilder.DropTable(
                name: "Peliculas",
                schema: "Disney");

            migrationBuilder.DropTable(
                name: "Personajes",
                schema: "Disney");

            migrationBuilder.DropTable(
                name: "Generos",
                schema: "Disney");
        }
    }
}
