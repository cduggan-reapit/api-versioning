using Asp.Versioning;

namespace Corvid.Api.Infrastructure.Versioning;

internal static class ConfigureVersioning
{
    public const string ApiVersionHeaderKey = "api-version";

    public static IServiceCollection ConfigureVersioningServices(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
            {
                options.ApiVersionReader = new HeaderApiVersionReader(ApiVersionHeaderKey);
                options.ReportApiVersions = false;
            })
            .AddApiExplorer(options => options.GroupNameFormat = "G")
            .AddMvc(options => options.Conventions.Add(new IncrementalApiVersionConvention()));

        return services;
    } 
}