using Embedings.Interfaces;
using Embedings.Services;
using Microsoft.EntityFrameworkCore;
using MSSqlServerDB;

namespace Embedings.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApplicationService(this IServiceCollection services)
    {
        services.AddScoped<IEmbeddingService, EmbeddingService>();
        services.AddScoped<IGPTService, GPTService>();
        services.AddScoped<IPineconeService, PineconeService>();
        services.AddScoped<IChatBotService, ChatBotService>();
        services.AddScoped<SimilaritySearchService>();

        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<EmbedingsDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        return services;
    }
}
