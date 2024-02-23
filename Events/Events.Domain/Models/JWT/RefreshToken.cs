namespace Events.Domain.Models.JWT
{
    public class RefreshToken
    {
        /// <summary>
        /// Username, can be used for usage tracking can optionally include other metadata, 
        /// such as user agent, ip address, device name, and so on
        /// </summary>
        public string UserName { get; set; }

        public string TokenString { get; set; }

        public DateTime ExpireAt { get; set; }

        public RefreshToken(string username, string tokenString, DateTime expireAt)
        {
            UserName = username;
            TokenString = tokenString;
            ExpireAt = expireAt;
        }
    }
}
