namespace ChallengeApi.Entities
{
    public class Productora
    {
        public int Id { get; set; } 
        public string Nombre { get; set; } = string.Empty;
        public DateOnly FechaFundacion { get; set; }
        // Relación inversa con Pelicula
        public ICollection<Pelicula> Peliculas { get; set; } = new List<Pelicula>();

    }
}
