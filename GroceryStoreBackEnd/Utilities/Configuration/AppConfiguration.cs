using GroceryStoreCore.Utilities.Configuration.Interfaces;

namespace GroceryStoreBackEnd.Utilities.Configuration
{
    public class AppConfiguration : IAppConfiguration
    {
        private readonly IConfiguration _configuration;

        public AppConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string JwtSecretKey => _configuration["Jwt:SecretKey"];

        public string JwtIssuer => _configuration["Jwt:Issuer"];

        public int JwtExpiryInMinutes => int.Parse(_configuration["Jwt:ExpiryInMinutes"]);

        public string JwtAudience => _configuration["Jwt:Audience"];
    }
}
