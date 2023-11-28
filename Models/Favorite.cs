using System.ComponentModel.DataAnnotations;

namespace MVCMovieInfo.Models
{
    public class Favorite
    {
        public int ID { get; set; } //Primary Key

        [Required]
        public int MovieID { get; set; } //Foreign Key (Movie)

        [Required]
        [StringLength(450)]
        public string? UserID { get; set; } //Relation a ASPNET_USERS (Id)

        //========================= Navigation Properties =================================
        public virtual Movie? Movie { get; set; }//A Favorite can only belong at most to one Movie 
    }
}
