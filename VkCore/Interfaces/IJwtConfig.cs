namespace VkCore.Interfaces
{
    public interface IJwtConfig
    {
        string JwtAudience { get; set; }
        string JwtSecretKey { get; set; }
        string JwtIssuer { get; set; }
    }
}
