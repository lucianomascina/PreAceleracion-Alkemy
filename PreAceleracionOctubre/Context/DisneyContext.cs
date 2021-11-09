using Microsoft.EntityFrameworkCore;
using PreAceleracionOctubre.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreAceleracionOctubre.Context
{
    public class DisneyContext : DbContext
    {
        private const string Schema = "Disney";
        public DisneyContext(DbContextOptions<DisneyContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema(Schema);

            modelBuilder.Entity<Personaje>().HasData(
                new Personaje()
                {
                    Id = 1,
                    Imagen = "img",
                    Nombre = "mickey",
                    Edad = 80,
                    Peso = 3,
                    Historia = "el raton",

                });
        }

        public DbSet<Personaje> Personajes { get; set; }
        public DbSet<Pelicula> Peliculas { get; set; }
        public DbSet<Genero> Generos { get; set; }

    }
}
