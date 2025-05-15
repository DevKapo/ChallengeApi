namespace ChallengeApi.DTOs
{
    public class PortadaDto
    {
        public int Id { get; set; }
        public string Ruta { get; set; } = string.Empty;
        public float Peso { get; set; }
        public int Ancho { get; set; }
        public int Alto { get; set; }
        public string Url { get; set; } = string.Empty;
    }
}
