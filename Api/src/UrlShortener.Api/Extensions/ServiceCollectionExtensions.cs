using UrlShortener.Core;
using UrlShortener.Core.Urls.Add;
using UrlShortener.Core.Urls.List;

namespace UrlShortener.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUrlFeature(this IServiceCollection services)
    {
        services.AddScoped<AddUrlHandler>();
        services.AddSingleton<TokenProvider>();
        services.AddScoped<ShortUrlGenerator>();

        return services;
    }

    public static IServiceCollection AddListUrlsFeature(this IServiceCollection services)
    {
        services.AddScoped<ListUrlsHandler>();
        return services;
    }
}
