namespace Embedings.Extensions;

public static class ConfigurationExtensions
{
    public static IConfigurationBuilder AddCustomConfiguration(this IConfigurationBuilder builder, WebApplicationBuilder webBuilder)
    {
        builder.AddUserSecrets<Program>();
        return builder;
    }
}