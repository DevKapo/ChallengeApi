namespace ChallengeApi.DTOs
{
    public class PeliculaCrearDto
    {
        public string Nombre { get; set; } = string.Empty;
        public int Duracion { get; set; }
        public int Puntuacion { get; set; }
        public decimal PresupuestoUsd { get; set; }
        public string ProductoraNombre { get; set; } = string.Empty;
        public string PortadaUrl { get; set; } = string.Empty;

        public List<string> Generos { get; set; } = new();
        public List<string> Actores { get; set; } = new();
    }
}
