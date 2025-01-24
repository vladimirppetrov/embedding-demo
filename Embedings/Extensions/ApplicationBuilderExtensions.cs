namespace Embedings.Extensions;

public static class ApplicationBuilderExtensions
{
    public static void AddApplicationConfiguration(this WebApplicationBuilder builder)
    {
        builder.Configuration.AddCustomConfiguration(builder);
        builder.Services.AddDatabase(builder.Configuration);
        builder.Services.AddApplicationService();
    }
}
