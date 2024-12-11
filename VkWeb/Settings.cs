using VkCore.Interfaces;

namespace VkWeb
{
    public class Settings : IConnectionStrings, IEmailConfig, IJwtConfig, IAwsConfig, IVkConfig
    {
        public string LocalhostPostgres { get; set; }
        public string AmazonRdsPostgres { get; set; }
        public string VkContactAddress { get; set; }
        public string JwtSecretKey { get; set; }
        public string JwtAudience { get; set; }
        public string JwtIssuer { get; set; }
        public string AwsProfile { get; set; }
        public string AwsRegion { get; set; }
        public int DefaultStudentLimit { get; set; }
    }
}
