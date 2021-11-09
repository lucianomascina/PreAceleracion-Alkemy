using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PreAceleracionOctubre.ViewModel.Pelicula
{
    public class PeliculaPostRequestViewModel
    {
        [Required]
        public string Imagen { get; set; }
       
        [Required]
        public string Titulo { get; set; }
        public DateTime FechaCreacion { get; set; }
     
        [Range(1,5)]
        public int Calificacion { get; set; }
        public List<int> PersonajesIds { get; set; }
    }
}
