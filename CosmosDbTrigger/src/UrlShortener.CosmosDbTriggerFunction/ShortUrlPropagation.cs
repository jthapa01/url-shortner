using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace UrlShortener.CosmosDbTriggerFunction
{
    public class ShortUrlPropagation(ILoggerFactory loggerFactory, Container container)
    {
        private readonly ILogger _logger = loggerFactory.CreateLogger<ShortUrlPropagation>();

        [Function("ShortUrlPropagation")]
        public async Task Run([CosmosDBTrigger(
            databaseName: "urls",
            containerName: "items",
            Connection = "CosmosDbConnection",
            LeaseContainerName = "leases",
            CreateLeaseContainerIfNotExists = true)] IReadOnlyList<UrlDocument>? input)
        {
            if (input is not { Count: > 0 }) return;

            foreach (var document in input)
            {
                _logger.LogInformation("Short url: {ShortUrl}", document.Id);
                try
                {
                    var cosmosDbDocument = new ShortenedUrlEntity(
                        document.LongUrl, 
                        document.Id, 
                        document.CreatedOn, 
                        document.CreatedBy);
                    
                    await container.UpsertItemAsync(cosmosDbDocument, new PartitionKey(document.CreatedBy));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error writing to Cosmos DB");
                    throw;
                }
            }
        }
    }

    public class UrlDocument
    {
        public string Id { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public string LongUrl { get; set; }
    }

    public class ShortenedUrlEntity(string longUrl, string shortUrl, DateTimeOffset createdOn, string createdBy)
    {
        public string LongUrl { get; } = longUrl;

        [JsonProperty(PropertyName = "id")] // Cosmos Db unique Identfier
        public string ShortUrl { get; } = shortUrl;

        public DateTimeOffset CreatedOn { get; } = createdOn;

        [JsonProperty(PropertyName = "PartitionKey")] // Cosmos Db partition key
        public string CreatedBy { get; } = createdBy;
    }
}