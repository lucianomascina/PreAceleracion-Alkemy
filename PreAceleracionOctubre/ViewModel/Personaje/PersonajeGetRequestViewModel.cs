using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreAceleracionOctubre.ViewModel.Personaje
{
    public class PersonajeGetRequestViewModel
    {
        public string name { get; set; }
        public int age { get; set; }
        public int peso { get; set; }
        public int movieId { get; set; }
    }
}
