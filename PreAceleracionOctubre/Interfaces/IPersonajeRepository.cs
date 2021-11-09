using PreAceleracionOctubre.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreAceleracionOctubre.Interfaces
{
    public interface IPersonajeRepository : IBaseRepository<Personaje>
    {
        Personaje GetPersonaje(int id);
        List<Personaje> GetPersonajes();

    }
}
