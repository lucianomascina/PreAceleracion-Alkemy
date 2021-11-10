using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PreAceleracionOctubre.Context;
using PreAceleracionOctubre.Entities;
using PreAceleracionOctubre.Interfaces;
using PreAceleracionOctubre.Repositories;
using PreAceleracionOctubre.ViewModel.Pelicula;
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
    public class PeliculaController : ControllerBase
    {
        private readonly IPeliculaRepository _peliculaRepository;
        private readonly IPersonajeRepository _personajeRepository;

        public PeliculaController(IPeliculaRepository peliculaRepository)
        {
            _peliculaRepository = peliculaRepository;
        }

        [HttpGet]
        [Route("movies")]
        [AllowAnonymous]
        public IActionResult Get([FromQuery]PeliculaGetRequestViewModel peliculaGetRequestViewModel)
        {
            var peliculas = _peliculaRepository.GetPeliculas();

            if (!string.IsNullOrEmpty(peliculaGetRequestViewModel.title))
            {
                peliculas = peliculas.Where(x => x.Titulo == peliculaGetRequestViewModel.title).ToList();
            }

            if(peliculaGetRequestViewModel.generoId != 0)
            {
                peliculas = peliculas.Where(x => x.genero.Id == peliculaGetRequestViewModel.generoId).ToList();
            }

            if (!string.IsNullOrEmpty(peliculaGetRequestViewModel.order))
            {
                switch(peliculaGetRequestViewModel.order)
                {
                    case "ASC":
                        peliculas = peliculas.OrderBy(p => p.FechaCreacion).ToList();
                        break;
                    case "DESC":
                        peliculas = peliculas.OrderByDescending(p => p.FechaCreacion).ToList();
                        break;
                    default:
                        break;
                }    
            }

            if (!peliculas.Any())
            {
                return NoContent();
            }
            
            var listado = peliculas.Select(x => new { x.Imagen, x.Titulo, x.FechaCreacion }).ToList();

            return Ok(listado);
        }

        [HttpGet]
        [Route("movie")]
        [AllowAnonymous]
        public IActionResult Get(int id)
        {
            var pelicula = _peliculaRepository.GetPelicula(id);

            if (pelicula == null)
                return NotFound($"la pelicula con id {id} no existe.");

            return Ok(pelicula);
        }

        [HttpPost]
        public IActionResult Post(PeliculaPostRequestViewModel peliculaPostViewModel)
        {
            var pelicula = new Pelicula
            {
                Imagen = peliculaPostViewModel.Imagen,
                Titulo = peliculaPostViewModel.Titulo,
                FechaCreacion = peliculaPostViewModel.FechaCreacion,
                Calificacion = peliculaPostViewModel.Calificacion,
            };
            if (peliculaPostViewModel.PersonajesIds.Any())
            {
                foreach (var p in peliculaPostViewModel.PersonajesIds)
                {
                    var personaje = _personajeRepository.GetEntity(p);

                    if (personaje != null)
                        pelicula.Personajes.Add(personaje);
                }
            }
            _peliculaRepository.Add(pelicula);

            return Ok(new PeliculaPostResponseViewModel
                {
                    Imagen = pelicula.Imagen,
                    Titulo = pelicula.Titulo
                });
        }

        [HttpPut]
        public IActionResult Put(PeliculaPutRequestViewModel peliculaPutViewModel)
        {
            var pelicula = _peliculaRepository.GetPelicula(peliculaPutViewModel.Id);

            if (pelicula == null)
                return NotFound($"la pelicula con id {peliculaPutViewModel.Id} no existe.");

            pelicula.Imagen = peliculaPutViewModel.Imagen;
            pelicula.Titulo = peliculaPutViewModel.Titulo;
            pelicula.FechaCreacion = peliculaPutViewModel.FechaCreacion;
            pelicula.Calificacion = peliculaPutViewModel.Calificacion;

            if (peliculaPutViewModel.PersonajesIds.Any())
            {
                foreach (var p in peliculaPutViewModel.PersonajesIds)
                {
                    var personaje = _personajeRepository.GetEntity(p);

                    if (personaje != null)
                        pelicula.Personajes.Add(personaje);
                }
            }

            _peliculaRepository.Update(pelicula);
            
            return Ok(new PeliculaPutResponseViewModel
            { 
                Imagen = pelicula.Imagen,
                Titulo = pelicula.Titulo
            });
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(int id)
        {
            var pelicula = _peliculaRepository.GetPelicula(id);
            
            if (pelicula == null)
                return NotFound($"la pelicula con id {id} no existe.");
            
            _peliculaRepository.Delete(id);
            
            return Ok("La pelicula se borró correctamente.");
        }
    }
}
