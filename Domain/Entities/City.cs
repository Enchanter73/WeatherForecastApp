
namespace Domain.Entities
{
    public class City
    {
        public int CityId { get; set; }

        public required string CityName { get; set; }

        public float? LatestTemperature { get; set; }

        public DateTime? LastUpdated { get; set; }
    }
}
