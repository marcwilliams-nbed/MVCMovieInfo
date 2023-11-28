using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MVCMovieInfo.Models
{
    public class Movie
    {
        public int MovieId { get; set; } //Primary Key
        [Required]
        public string? Title { get; set; }//? nullable string

        [DataType(DataType.Date)] //Date only without time (Database, DatePicker-View) 
        [Display(Name = "Release Date")]
        public DateTime ReleaseDate { get; set; }

        //Part 9:  Update some db fields:  will require new migration
        [Range(1, 100)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")] //SQL Server Money datatype
        public decimal Price { get; set; }
        public string? Rating { get; set; }

        //Part 2:  Add the relationships between entities 
        //Foreign key to Genre

        [Display(Name = "Genre")]
        public int GenreId { get; set; }

        //========================= Navigation Properties =================================
        public virtual Genre? Genre { get; set; }//A Movie can only belong at most to one Genre 


        //======================= New Addition: Favorite movies  =========================
        public virtual ICollection<Favorite>? Favorites { get; set; }
    }
}
