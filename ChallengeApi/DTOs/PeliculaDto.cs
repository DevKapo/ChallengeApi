namespace ChallengeApi.DTOs
{
    public class PeliculaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int Duracion { get; set; }
        public decimal Puntuacion { get; set; }
        public decimal PresupuestoUsd { get; set; }

        // Relación con Productora
        public string ProductoraNombre { get; set; } = string.Empty;

        // Relación con Portada
        public PortadaDto Portada { get; set; } = new();

        // Relación con Generos
        public List<string> Generos { get; set; } = new();
        // Relación con Actores
        public List<string> Actores { get; set; } = new();
    }
}

