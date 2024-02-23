
namespace Events.Domain.Models.JWT 
{ 
    public class LoginResult
    {
        public long ID { get; set; } = default!;
        public int RoleID { get; set; } = default!;
        public string UserName { get; set; } = default!;

        public string? Role { get; set; }

        public string? OriginalUserName { get; set; }

        public string AccessToken { get; set; } = default!;

        public string? RefreshToken { get; set; }

    }
}
