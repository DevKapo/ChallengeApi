namespace ChallengeApi.DTOs
{
    public class ActorDto
    {

        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public DateOnly FechaNac { get; set; }
    }
}
