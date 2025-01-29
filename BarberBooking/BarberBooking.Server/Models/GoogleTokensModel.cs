using System.Text.Json.Serialization;

namespace BarberBooking.Server.Models
{
    public class GoogleTokensModel : GoogleRefreshTokenResponse
    {
        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
