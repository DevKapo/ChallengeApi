namespace ChallengeApi.DTOs
{
    public class PortadaDto
    {
        public int Id { get; set; }
        public string Ruta { get; set; } = string.Empty;
        public decimal Peso { get; set; }
        public int Ancho { get; set; }
        public int Alto { get; set; }
        public int PeliculaID { get; set; }
        public string Url { get; set; } = string.Empty;
        public string PeliculaNombre { get; set; } = string.Empty;
    }
}
