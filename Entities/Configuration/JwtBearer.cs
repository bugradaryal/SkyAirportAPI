namespace Entities.Configuration
{
    public class JwtBearer
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }
        public int DurationInMinutes { get; set; }
    }
}
