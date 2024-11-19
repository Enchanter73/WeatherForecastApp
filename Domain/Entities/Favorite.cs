using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Favorite
    {
        public int FavoriteID { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [ForeignKey("City")]
        public int CityId { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.Now;

        public User? User { get; set; }
        public City? City { get; set; }
    }
}
