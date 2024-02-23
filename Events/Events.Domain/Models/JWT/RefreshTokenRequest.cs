namespace Events.Domain.Models.JWT
{
    public class RefreshTokenRequest
    {
        public string Username { get; set; } = default!;
        public string ClientID { get; set; } = default!;
        public string ClientSecret { get; set; } = default!;
        public string RefreshToken { get; set; } = default!;
    }
}
