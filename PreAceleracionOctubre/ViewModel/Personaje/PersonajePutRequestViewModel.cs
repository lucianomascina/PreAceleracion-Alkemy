using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PreAceleracionOctubre.ViewModel.Personaje
{
    public class PersonajePutRequestViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Imagen { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public int Edad { get; set; }

        public int Peso { get; set; }
        public string Historia { get; set; }
        public List<int> PeliculasIds { get; set; }
    }
}
