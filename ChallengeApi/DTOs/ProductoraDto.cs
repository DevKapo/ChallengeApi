namespace ChallengeApi.DTOs
{
    public class ProductoraDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public DateOnly FechaFundacion { get; set; }
    }
}
