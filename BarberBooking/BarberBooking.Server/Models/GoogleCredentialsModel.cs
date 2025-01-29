using System.Text.Json.Serialization;

namespace BarberBooking.Server.Models
{
    public class GoogleCredentialsModel
    {
        [JsonPropertyName("client_id")]
        public string ClientId { get; set; } = string.Empty;

        [JsonPropertyName("client_secret")]
        public string ClientSecret { get; set; } = string.Empty;
    }
}
