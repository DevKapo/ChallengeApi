
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChallengeApi.DTOs;
using ChallengeApi.Data;
using ChallengeApi.Entities;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace ChallengeApi.Controllers
{
    [Route("odata/Actores")]
    [ApiController]
    public class ActorController : ODataController
    {
        private readonly AppDbContext _context;

        public ActorController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActorDto>>> GetActorDto()
        {
            var actores = await _context.Actores
                .Select(a => new ActorDto
                {

                    Id = a.Id,
                    Nombre = a.Nombre,
                    Apellido = a.Apellido,
                    FechaNac = a.FechaNac
                })
                .ToListAsync();
            return Ok(actores);
        }

        [HttpGet("{key}")]
        public async Task<ActionResult<ActorDto>> GetActorDto(int key)
        {
            var actor = await _context.Actores.FindAsync(key);

            if (actor == null)
            {
                return NotFound();
            }

            var actorDto = new ActorDto
            {
                Id = actor.Id,
                Nombre = actor.Nombre,
                Apellido = actor.Apellido,
                FechaNac = actor.FechaNac
            };
            return Ok(actorDto);
        }


        [HttpPost]
        public async Task<ActionResult<ActorDto>> PostActor([FromBody] ActorDto actorDto)
        {
            var actor = new Actor
            {
                Nombre = actorDto.Nombre,
                Apellido = actorDto.Apellido,
                FechaNac = actorDto.FechaNac
            };

            _context.Actores.Add(actor);
            await _context.SaveChangesAsync();

            // Actualizás el DTO con el ID generado
            actorDto.Id = actor.Id;

            return CreatedAtAction(nameof(GetActorDto), new { key = actor.Id }, actorDto);
        }



       
        [HttpPut("{key}")]
        public async Task<IActionResult> PutActorDto(int key, ActorDto actorDto)
        {
            var actor = await _context.Actores.FindAsync(key);
            if (actor == null)
            {
                return NotFound();
            }

            actor.Nombre = actorDto.Nombre;
            actor.Apellido = actorDto.Apellido;
            actor.FechaNac = actorDto.FechaNac;

            await _context.SaveChangesAsync();

            return NoContent(); // 204 - Actualización exitosa, sin contenido para devolver
        }

        [HttpDelete("{key}")]
        public async Task<IActionResult> DeleteActor(int key)
        {
            var actor = await _context.Actores.FindAsync(key);
            if (actor == null)
            {
                return NotFound();
            }

            _context.Actores.Remove(actor);
            await _context.SaveChangesAsync();

            return NoContent(); // 204 - Borrado exitoso
        }

    }
}
