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
    public class PersonajeRepository : BaseRepository<Personaje, DisneyContext>, IPersonajeRepository
    {
        public PersonajeRepository(DisneyContext dbContext) : base(dbContext)
        {
        }
        public Personaje GetPersonaje(int id)
        {
            return DbSet.Include(x => x.Peliculas).FirstOrDefault(x => x.Id == id);
        }
        public List<Personaje> GetPersonajes()
        {
            return DbSet.Include(x => x.Peliculas).ToList();
        }
    }
}
