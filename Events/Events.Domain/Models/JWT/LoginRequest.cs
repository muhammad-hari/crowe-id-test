using System.ComponentModel.DataAnnotations;

namespace Events.Domain.Models.JWT
{
    public class LoginRequest
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ClientID { get; set; }

        [Required]
        public string ClientSecret { get; set; }

        public LoginRequest(string username, string password, string clientID, string clientSecret)
        {
            UserName = username;
            Password = password;    
            ClientID = clientID;    
            ClientSecret = clientSecret;
        }
    }
}
