using System.ComponentModel.DataAnnotations;

namespace MVCMovieInfo.Models
{
    public class Genre
    {

        public int GenreId { get; set; }
        [Required]
        public string? Name { get; set; }

        public virtual ICollection<Movie>? Movies { get; set; }
    }
}
