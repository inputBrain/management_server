namespace Management.Server.Host
{
    public sealed class JwtSettings
    {
        public string SecretKey { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }
    }
}