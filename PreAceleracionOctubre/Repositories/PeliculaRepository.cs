using Microsoft.EntityFrameworkCore;
using PreAceleracionOctubre.Context;
using PreAceleracionOctubre.Entities;
using PreAceleracionOctubre.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreAceleracionOctubre.Repositories
{
    public class PeliculaRepository : BaseRepository<Pelicula, DisneyContext>, IPeliculaRepository
    {


        public PeliculaRepository(DisneyContext dbContext) : base(dbContext)
        {
        }
        public Pelicula GetPelicula(int id)
        {
            return DbSet.Include(x => x.Personajes).Include(y => y.genero).FirstOrDefault(x => x.Id == id);
        }
        public List<Pelicula> GetPeliculas()
        {
            return DbSet.Include(x => x.Personajes).Include(y => y.genero).ToList();
        }
    }
}
