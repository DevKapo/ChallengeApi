using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChallengeApi.DTOs;
using ChallengeApi.Data;
using ChallengeApi.Entities;
using AutoMapper;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Query;

namespace ChallengeApi.Controllers
{

    [Route("odata/Peliculas")]
    [ApiController]
    public class PeliculaController : ODataController
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public PeliculaController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper; 
        }

        [EnableQuery]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PeliculaDto>>> GetPeliculaDto()
        {
            var peliculas = await _context.Peliculas
                .Include(p => p.Productora)
                .Include(p => p.Portada)
                .Include(p => p.Generos)
                .Include(p => p.Actores)
                .ToListAsync();



            return _mapper.Map<List<PeliculaDto>>(peliculas);
        }

        // GET: api/PeliculaDto/5
        [HttpGet("{key}")]
        public async Task<ActionResult<PeliculaDto>> GetPelicula(int key)
        {
            var pelicula = await _context.Peliculas
                .Include(p => p.Productora)
                .Include(p => p.Portada)
                .Include(p => p.Generos)
                .Include(p => p.Actores)
                .FirstOrDefaultAsync(p => p.Id == key);

            if (pelicula == null)
            {
                return NotFound();
            }
            
            return _mapper.Map<PeliculaDto>(pelicula);
        }

        // POST: api/PeliculaDto
        [HttpPost]
        public async Task<IActionResult> PostPelicula(PeliculaCrearDto dto)
        {
            var pelicula = new Pelicula
            {
                Nombre = dto.Nombre,
                Duracion = dto.Duracion,
                Puntuacion = dto.Puntuacion,
                PresupuestoUsd = dto.PresupuestoUsd,
                Generos = new List<Genero>(),
                Actores = new List<Actor>(),
                Portada = new Portada
                {
                    Ruta = dto.Portada.Ruta,
                    Peso = dto.Portada.Peso,
                    Ancho = dto.Portada.Ancho,
                    Alto = dto.Portada.Alto,
                    Url = dto.Portada.Url
                }
            };

            // Manejo productora
            var productora = await _context.Productoras.FirstOrDefaultAsync(p => p.Nombre == dto.ProductoraNombre);
            if (productora == null)
            {
                productora = new Productora { Nombre = dto.ProductoraNombre };
                _context.Productoras.Add(productora);
                await _context.SaveChangesAsync(); // Guarda para generar ID si lo necesitas
            }
            pelicula.Productora = productora;

            // Manejo géneros
            foreach (var generoNombre in dto.Generos.Distinct())
            {
                var genero = await _context.Generos.FirstOrDefaultAsync(g => g.Nombre == generoNombre);
                if (genero == null)
                {
                    genero = new Genero { Nombre = generoNombre };
                    _context.Generos.Add(genero);
                    await _context.SaveChangesAsync();
                }
                pelicula.Generos.Add(genero);
            }

            // Manejo actores
            foreach (var actorNombre in dto.Actores.Distinct())
            {
                var actor = await _context.Actores.FirstOrDefaultAsync(a => a.Nombre == actorNombre);
                if (actor == null)
                {
                    actor = new Actor { Nombre = actorNombre };
                    _context.Actores.Add(actor);
                    await _context.SaveChangesAsync();
                }
                pelicula.Actores.Add(actor);
            }

            _context.Peliculas.Add(pelicula);
            await _context.SaveChangesAsync();

            // Ahora que la pelicula tiene Id asignado, vinculamos el PeliculaID en portada
            pelicula.Portada.PeliculaID = pelicula.Id;
            _context.Portadas.Update(pelicula.Portada); 
            await _context.SaveChangesAsync();

            var peliculaConPortada = await _context.Peliculas
                .Include(p => p.Portada)
                .Include(p => p.Productora)
                .Include(p => p.Generos)
                .Include(p => p.Actores)
                .FirstOrDefaultAsync(p => p.Id == pelicula.Id);

            

            var peliculaDto = _mapper.Map<PeliculaDto>(peliculaConPortada);
            return CreatedAtAction(nameof(GetPelicula), new { key = pelicula.Id }, peliculaDto);
        }


        [HttpPut("{key}")]
        public async Task<IActionResult> PutPelicula(int key, PeliculaDto dto)
        {
            if (key != dto.Id)
            {
                return BadRequest("El ID en la URL no coincide con el del cuerpo.");
            }

            var peliculaExistente = await _context.Peliculas
                .Include(p => p.Generos)
                .Include(p => p.Actores)
                .Include(p => p.Productora)
                .Include(p => p.Portada)
                .FirstOrDefaultAsync(p => p.Id == key);

            
            if (peliculaExistente == null)
            {
                return NotFound();
            }

            peliculaExistente.Nombre = dto.Nombre;
            peliculaExistente.Duracion = dto.Duracion;
            peliculaExistente.Puntuacion = dto.Puntuacion;
            peliculaExistente.PresupuestoUsd = dto.PresupuestoUsd;

            peliculaExistente.Actores = await _context.Actores
                .Where(a => dto.Actores.Contains(a.Nombre))
                .ToListAsync();

            peliculaExistente.Generos = await _context.Generos
                .Where(g => dto.Generos.Contains(g.Nombre))
                .ToListAsync();


            var productora = await _context.Productoras
                .FirstOrDefaultAsync(p => p.Nombre == dto.ProductoraNombre);

            if (productora == null)
            {
                productora = new Productora { Nombre = dto.ProductoraNombre };
                _context.Productoras.Add(productora);
                await _context.SaveChangesAsync(); // Guardar para generar el ID
            }

            peliculaExistente.Productora = productora;

            if (peliculaExistente.Portada == null)
            {
                peliculaExistente.Portada = new Portada { Url = dto.Portada.Url };
            }
            else
            {
                peliculaExistente.Portada.Url = dto.Portada.Url;
            }

            await _context.SaveChangesAsync();

            return NoContent(); // 204 - Actualización exitosa, sin contenido para devolver
        }


        [HttpDelete("{key}")]
        public async Task<IActionResult> DeletePelicula(int key)
        {
            var pelicula = await _context.Peliculas
              .Include(p => p.Portada)
              .Include(p => p.Generos)
              .Include(p => p.Actores)
              .FirstOrDefaultAsync(p => p.Id == key);
            if (pelicula == null)
            {
                return NotFound();
            }

            _context.Peliculas.Remove(pelicula);
            await _context.SaveChangesAsync();

            return NoContent(); // 204 - Borrado exitoso
        }

    }
}
