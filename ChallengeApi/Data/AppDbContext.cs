using ChallengeApi.Entities;
using Microsoft.EntityFrameworkCore;
using ChallengeApi.DTOs;

namespace ChallengeApi.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Pelicula> Peliculas => Set<Pelicula>();
        public DbSet<Genero> Generos => Set<Genero>();
        public DbSet<Actor> Actores => Set<Actor>();
        public DbSet<Productora> Productoras => Set<Productora>();
        public DbSet<Portada> Portadas => Set<Portada>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Relacion muchos a muchos es decir Pelicula <-> Genero 

            modelBuilder.Entity<Pelicula>()
                .HasMany(p => p.Generos)
                .WithMany(g => g.Peliculas);

            //Relacion muchos a muchos es decir Pelicula <-> Actor 
            modelBuilder.Entity<Pelicula>()
                .HasMany(p => p.Actores)
                .WithMany(a => a.Peliculas);


            //Relacion uno a uno Pelicula <-> Portada
            modelBuilder.Entity<Pelicula>()
                .HasOne(p => p.Portada)
                .WithOne(p => p.Pelicula)
                .HasForeignKey<Portada>(p => p.PeliculaID);






        }
        public DbSet<ChallengeApi.DTOs.ProductoraDto> ProductoraDto { get; set; } = default!;
    }
}
