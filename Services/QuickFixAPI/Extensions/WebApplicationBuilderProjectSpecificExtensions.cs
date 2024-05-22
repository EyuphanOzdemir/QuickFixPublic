using MongoDB.Driver;

namespace QuickFixAPI.Extensions
{
    public static class WebApplicationBuilderProjectSpecificExtensions
    {
        public static WebApplicationBuilder AddMongoDatabase(this WebApplicationBuilder builder, string connectionString, string databaseName)
        {
            var settings = MongoClientSettings.FromConnectionString(connectionString);
            settings.ConnectTimeout = TimeSpan.FromSeconds(60);
            var client = new MongoClient(settings);
            var database = client.GetDatabase(databaseName);
            builder.Services.AddSingleton<IMongoDatabase>(database);
            return builder;
        }
    }
}
