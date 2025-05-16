namespace ChallengeApi.Entities
{
    public class Pelicula
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int Duracion { get; set; }
        public decimal Puntuacion { get; set; }
        public decimal PresupuestoUsd { get; set; }
        // Relación con Productora
        public int ProductoraId { get; set; }
        public Productora Productora { get; set; } = null!;
        // Relación con Portada
        public Portada Portada { get; set; } = new();
        // Relación con Géneros (puede tener varios)
        public ICollection<Genero> Generos { get; set; } = new List<Genero>();
        // Relación con Actores (puede tener varios)
        public ICollection<Actor> Actores { get; set; } = new List<Actor>();
    }
}
