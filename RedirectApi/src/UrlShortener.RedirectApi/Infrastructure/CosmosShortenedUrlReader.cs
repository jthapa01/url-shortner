using Microsoft.Azure.Cosmos;

namespace UrlShortener.RedirectApi.Infrastructure;

public class CosmosShortenedUrlReader(Container container) : IShortenedUrlReader
{
    public async Task<ReadLongUrlResponse> GetLongUrlAsync(string shortUrl, CancellationToken cancellationToken)
    {
        try
        {
            var record = await container.ReadItemAsync<CosmosUrlRecord>(
                shortUrl,
                new PartitionKey(shortUrl[..1]),
                cancellationToken: cancellationToken);

            return record switch
            {
                { Resource: not null } => new ReadLongUrlResponse(true, record.Resource.LongUrl),
                _ => new ReadLongUrlResponse(false, null)
            };
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return new ReadLongUrlResponse(false, null);
        }
    }

    record CosmosUrlRecord(string LongUrl);
}