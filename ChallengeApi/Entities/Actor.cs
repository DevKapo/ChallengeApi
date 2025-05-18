namespace ChallengeApi.Entities
{
    public class Actor
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public DateOnly FechaNac { get; set; } 
        public ICollection<Pelicula> Peliculas { get; set; } = new List<Pelicula>();


    }
}
