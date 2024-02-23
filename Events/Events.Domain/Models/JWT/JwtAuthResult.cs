namespace Events.Domain.Models.JWT
{
    public class JwtAuthResult
    {
        public string? AccessToken { get; set; }

        public RefreshToken? RefreshToken { get; set; }
    }
}
