using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PreAceleracionOctubre.Context;
using PreAceleracionOctubre.Entities;
using PreAceleracionOctubre.Interfaces;
using PreAceleracionOctubre.Repositories;
using PreAceleracionOctubre.ViewModel.Personaje;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreAceleracionOctubre.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PersonajeController : ControllerBase
    {
        private readonly IPersonajeRepository _personajeRepository;
        private readonly PeliculaRepository _peliculaRepository;

        public PersonajeController(IPersonajeRepository personajeRepository)
        {
            _personajeRepository = personajeRepository;
        }

        [HttpGet]
        [Route("characters")]
        [AllowAnonymous]
        public IActionResult Get([FromQuery]PersonajeGetRequestViewModel personajeGetRequestViewModel)
        {
            var personajes = _personajeRepository.GetPersonajes();

            if (!string.IsNullOrEmpty(personajeGetRequestViewModel.name))
                personajes = personajes.Where(x => x.Nombre == personajeGetRequestViewModel.name).ToList();
            
            if(personajeGetRequestViewModel.age != 0)
                personajes = personajes.Where(x => x.Edad == personajeGetRequestViewModel.age).ToList();

            if (personajeGetRequestViewModel.peso != 0)
                personajes = personajes.Where(x => x.Peso == personajeGetRequestViewModel.peso).ToList();

            if(personajeGetRequestViewModel.movieId != 0)
                personajes = personajes.Where(x => x.Peliculas.FirstOrDefault(x => x.Id == personajeGetRequestViewModel.movieId) != null).ToList();

            if (!personajes.Any())
                return NoContent();

            return Ok(personajes);

        }

        [HttpPost]
        public IActionResult Post(PersonajePostRequestViewModel personajePostViewModel)
        {
            var personaje = new Personaje
            {
                Imagen = personajePostViewModel.Imagen,
                Nombre = personajePostViewModel.Nombre,
                Edad = personajePostViewModel.Edad,
                Peso = personajePostViewModel.Peso,
                Historia = personajePostViewModel.Historia
            };
            if (personajePostViewModel.PeliculasIds.Any())
            {
                foreach(var p in personajePostViewModel.PeliculasIds)
                {
                    var pelicula = _peliculaRepository.GetEntity(p);

                    if (pelicula != null)
                        personaje.Peliculas.Add(pelicula);
                }
            }

            _personajeRepository.Add(personaje);
            
            return Ok(new PersonajePostResponseViewModel { 
                Imagen = personaje.Imagen,
                Nombre = personaje.Nombre
            });
        }

        [HttpPut]
        public IActionResult Put(PersonajePutRequestViewModel personajePutViewModel)
        {
            var personaje = _personajeRepository.GetPersonaje(personajePutViewModel.Id);

            if(personaje == null)
                return NotFound($"el personaje con id {personajePutViewModel.Id} no existe.");

            personaje.Imagen = personajePutViewModel.Imagen;
            personaje.Nombre = personajePutViewModel.Nombre;
            personaje.Edad = personajePutViewModel.Edad;
            personaje.Peso = personajePutViewModel.Peso;
            personaje.Historia = personajePutViewModel.Historia;

            if (personajePutViewModel.PeliculasIds.Any())
            {
                foreach (var p in personajePutViewModel.PeliculasIds)
                {
                    var pelicula = _peliculaRepository.GetEntity(p);

                    if (pelicula != null)
                        personaje.Peliculas.Add(pelicula);
                }
            }

            _personajeRepository.Update(personaje);
            return Ok(new PersonajePutResponseViewModel { 
                Imagen = personaje.Imagen,
                Nombre = personaje.Nombre
            });
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(int id)
        {
            var personaje = _personajeRepository.GetPersonaje(id);
            if (personaje == null)
                return NotFound($"el personaje con id {id} no existe.");
            _personajeRepository.Delete(id);
            return Ok();
        }
    }
}
