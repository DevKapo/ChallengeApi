namespace ChallengeApi.Entities
{
    public class Portada
    {
        public int Id { get; set; }
        public string Ruta { get; set; } = string.Empty;
        public decimal Peso { get; set; }
        public int Ancho { get; set; }
        public int Alto { get; set; }
        public int PeliculaID { get; set; }

        // Relación de navegación EF Core
        public Pelicula Pelicula { get; set; } = null!;

        // URL calculada en DTO o servicio, no almacenada
        public string Url { get; set; } = string.Empty;
    }
}
