using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PreAceleracionOctubre.Entities;
using PreAceleracionOctubre.Interfaces;
using PreAceleracionOctubre.ViewModel.Genero;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreAceleracionOctubre.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class GeneroController : ControllerBase
    {
        private readonly IGeneroRepository _generoRepository;

        public GeneroController(IGeneroRepository generoRepository)
        {
            _generoRepository = generoRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get()
        {
            var generos = _generoRepository.GetGeneros();

            if (!generos.Any())
                return NoContent();

            return Ok(generos);
        }

        [HttpPost]
        public IActionResult Post(GeneroPostRequestViewModel generoviewmodel)
        {
            var genero = new Genero
            {
                Nombre = generoviewmodel.Nombre,
                Imagen = generoviewmodel.Imagen,
            };

            _generoRepository.Add(genero);
            return Ok(genero);
        }

        [HttpPut]
        public IActionResult Put(Genero gen)
        {
            var genero = _generoRepository.GetGenero(gen.Id);

            if (genero == null)
                return NotFound($"el genero con id {gen.Id} no existe.");

            genero.Nombre = gen.Nombre;
            genero.Imagen = gen.Imagen;

            _generoRepository.Update(genero);
            return Ok(genero);
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(int id)
        {
            var genero = _generoRepository.GetGenero(id);
            if (genero == null)
                return NotFound($"el genero con id {id} no existe.");
            _generoRepository.Delete(id);
            return Ok();
        }
    }
}
