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
    public class GeneroRepository : BaseRepository<Genero, DisneyContext>, IGeneroRepository
    {
        public GeneroRepository(DisneyContext dbContext) : base(dbContext)
        {
        }
        public Genero GetGenero(int id)
        {
            return DbSet.Include(x => x.Peliculas).FirstOrDefault(x => x.Id == id);
        }
        public List<Genero> GetGeneros()
        {
            return DbSet.Include(x => x.Peliculas).ToList();
        }
    }
}
