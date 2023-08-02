namespace GroceryStoreCore.Utilities.Configuration.Interfaces
{
    public interface IAppConfiguration
    {
        string JwtSecretKey { get; }
        string JwtIssuer { get; }
        int JwtExpiryInMinutes { get; }
        string JwtAudience { get; }
    }
}
