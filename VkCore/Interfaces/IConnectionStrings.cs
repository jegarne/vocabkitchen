namespace VkCore.Interfaces
{
    public interface IConnectionStrings
    {
        string AmazonRdsPostgres { get; set; }
        string JwtSecretKey { get; set; }
    }
}
