namespace MarketApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConfig(this IServiceCollection services,
        IConfiguration configuration)
    {
        return services;
    }
    public static IServiceCollection AddService(this IServiceCollection services)
    {
        return services;
    }
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        return services;
    }
}