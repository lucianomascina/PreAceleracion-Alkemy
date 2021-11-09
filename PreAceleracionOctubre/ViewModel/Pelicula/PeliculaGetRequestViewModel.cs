using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreAceleracionOctubre.ViewModel.Pelicula
{
    public class PeliculaGetRequestViewModel
    {
        public string title { get; set; }
        public int generoId { get; set; }
        public string order { get; set; }
    }
}
