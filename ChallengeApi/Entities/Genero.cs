namespace ChallengeApi.Entities
{
    public class Genero
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public ICollection<Pelicula> Peliculas { get; set; } = new List<Pelicula>();
    }
}
