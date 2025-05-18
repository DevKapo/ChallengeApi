namespace ChallengeApi.DTOs
{
    public class PeliculaCrearDto
    {
        public string Nombre { get; set; } = string.Empty;
        public int Duracion { get; set; }
        public decimal Puntuacion { get; set; } 
        public decimal PresupuestoUsd { get; set; }
        public string ProductoraNombre { get; set; } = string.Empty;
        public string PortadaUrl { get; set; } = string.Empty;

        public PortadaDto Portada { get; set; } = new();

        public List<string> Generos { get; set; } = new();
        public List<string> Actores { get; set; } = new();
    }
}
