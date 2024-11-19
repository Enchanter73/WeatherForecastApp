
namespace Application.Models
{
    public class WeatherRequest
    {
        public string City { get; set; }
        public TaskCompletionSource<double?> CompletionSource { get; set; }
    }
}
