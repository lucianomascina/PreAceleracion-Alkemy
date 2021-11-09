using PreAceleracionOctubre.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreAceleracionOctubre.Interfaces
{
    public interface IGeneroRepository : IBaseRepository<Genero>
    {
        Genero GetGenero(int id);
        List<Genero> GetGeneros();
    }
}
