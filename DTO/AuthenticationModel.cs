namespace DTO
{
    public class AuthenticationModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
