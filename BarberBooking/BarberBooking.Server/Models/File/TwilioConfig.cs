using System.Text.Json.Serialization;

namespace BarberBooking.Server.Models.File
{
    public class TwilioConfig
    {
        [JsonPropertyName("account_id")]
        public string AccountId { get; set; } = string.Empty;
        [JsonPropertyName("auth_token")]
        public string AuthToken {  get; set; } = string.Empty;
        [JsonPropertyName("twilio_number")]
        public string TwilioNumber { get; set; } = string.Empty;
    }
}
